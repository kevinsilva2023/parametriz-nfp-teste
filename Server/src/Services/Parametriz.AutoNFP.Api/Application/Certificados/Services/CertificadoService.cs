using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Certificados;
using Parametriz.AutoNFP.Core.Configs;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.Usuarios;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Parametriz.AutoNFP.Api.Application.Certificados.Services
{
    public class CertificadoService : BaseService, ICertificadoService
    {
        private readonly int _keySize = 256;
        private readonly int _saltSize = 16;

        private readonly ICertificadoRepository _certificadoRepository;
        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly AppConfig _appConfig;

        public CertificadoService(IAspNetUser user,
                                 IUnitOfWork uow,
                                 Notificador notificador,
                                 ICertificadoRepository certificadoRepository,
                                 IOptions<AppConfig> options,
                                 IVoluntarioRepository voluntarioRepository)
            : base(user, uow, notificador)
        {
            _certificadoRepository = certificadoRepository;
            _appConfig = options.Value;
            _voluntarioRepository = voluntarioRepository;
        }

        private async Task ValidarCertificado(Certificado certificado)
        {
            await ValidarEntidade(new CertificadoValidation(), certificado);
        }

        private void CertificadoValido(Certificado certificado)
        {
            if (certificado.ValidoAte.Date < DateTime.Now.Date)
                NotificarErro("Certificado vencido.");
        }

        private async Task CertificadoExiste(Guid voluntarioId)
        {
            if (!await _certificadoRepository.ExisteNoVoluntario(voluntarioId))
                NotificarErro("Certificado não encontrado.");
        }

        private async Task VoluntarioCpfIgualAoCertificado(Certificado certificado)
        {
            if (!await _voluntarioRepository
                .CpfPertenceAoVoluntarioId(VoluntarioId, ExtrairCpnjCpfDoCommonName(certificado.Requerente), InstituicaoId))
                    NotificarErro("Certificado não pertence ao vonluntário.");
        }

        private async Task<bool> CertificadoAptoParaCadastrar(Certificado certificado)
        {
            await ValidarCertificado(certificado);
            CertificadoValido(certificado);
            await VoluntarioCpfIgualAoCertificado(certificado);
            
            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(CadastrarCertificadoViewModel cadastrarCertificadoViewModel)
        {
            var dataByteArray = Convert.FromBase64String(cadastrarCertificadoViewModel.Upload);

            X509Certificate2 certificadoDigital;

            try
            {
                certificadoDigital = new X509Certificate2(dataByteArray, cadastrarCertificadoViewModel.Senha);
            }
            catch (CryptographicException)
            {
                return NotificarErro("Senha incorreta.");
            }
            catch (Exception)
            {
                return NotificarErro("Erro desconhecido. Tente novamente");
            }

            //if (!certificadoDigital.Verify())
            //    return NotificarErro("Certificado inválido.");

            var pepper = Encoding.UTF8.GetBytes(_appConfig.SecrectKey);

            var senhaCripto = Encrypt(cadastrarCertificadoViewModel.Senha, VoluntarioId.ToString(), pepper);

            var certificado = new Certificado(Guid.NewGuid(), VoluntarioId, ExtrairCommonName(certificadoDigital.Subject),  
                certificadoDigital.NotBefore, certificadoDigital.NotAfter,  ExtrairCommonName(certificadoDigital.Issuer), 
                dataByteArray, senhaCripto);

            if (!await CertificadoAptoParaCadastrar(certificado))
                return false;

            if (await _certificadoRepository.ExisteNoVoluntario(VoluntarioId))
                await _certificadoRepository.Excluir(VoluntarioId);

            await _certificadoRepository.CadastrarAsync(certificado);

            await Commit();

            return CommandEhValido();
        }

        private string ExtrairCommonName(string texto)
        {
            var split = texto.Split(',');
            var cn = split.SingleOrDefault(cn => cn.StartsWith("CN="));

            return cn?.Substring(3) ?? string.Empty;
        }

        private string ExtrairNomeDoCommonName(string texto)
        {
            var cn = ExtrairCommonName(texto);

            if (string.IsNullOrEmpty(cn))
                return string.Empty;

            var split = cn.Split(":");

            return split.FirstOrDefault();
        }

        private string ExtrairCpnjCpfDoCommonName(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            var split = texto.Split(":");

            return split.LastOrDefault();
        }

        private async Task<bool> CertificadoAptoParaExcluir(Guid voluntarioId)
        {
            await CertificadoExiste(voluntarioId);

            return CommandEhValido();
        }

        public async Task<bool> Excluir(Guid voluntarioId)
        {
            if (!await CertificadoAptoParaExcluir(voluntarioId))
                return false;

            await _certificadoRepository.Excluir(voluntarioId);

            await Commit();

            return CommandEhValido();
        }

        private byte[] Encrypt(string plainText, string password, byte[] pepper)
        {
            byte[] salt = GenerateRandomSalt();

            var key = DeriveKey(password, salt, pepper);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        var encryptedBytes = memoryStream.ToArray();

                        var result = new byte[_saltSize + aes.IV.Length + encryptedBytes.Length];
                        Buffer.BlockCopy(salt, 0, result, 0, _saltSize);
                        Buffer.BlockCopy(aes.IV, 0, result, _saltSize, aes.IV.Length);
                        Buffer.BlockCopy(encryptedBytes, 0, result, _saltSize + aes.IV.Length, encryptedBytes.Length);

                        return result;
                    }
                }
            }
        }

        private byte[] DeriveKey(string password, byte[] salt, byte[] pepper)
        {
            var combinedPassword = Encoding.UTF8.GetBytes(password).Concat(pepper).ToArray();

            return Rfc2898DeriveBytes.Pbkdf2(
                combinedPassword,
                salt,
                iterations: 100000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: _keySize / 8);
        }

        private byte[] GenerateRandomSalt()
        {
            var salt = new byte[_saltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
    }
}
