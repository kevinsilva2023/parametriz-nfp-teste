using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.Api.Configs;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Domain.Core.Enums;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Services
{
    public class VoluntarioService : BaseService, IVoluntarioService
    {
        private readonly int _keySize = 256;
        private readonly int _saltSize = 16;

        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly AppConfig _appConfig;

        public VoluntarioService(IAspNetUser user,
                                 IUnitOfWork uow,
                                 Notificador notificador,
                                 IVoluntarioRepository voluntarioRepository,
                                 IOptions<AppConfig> options)
            : base(user, uow, notificador)
        {
            _voluntarioRepository = voluntarioRepository;
            _appConfig = options.Value;
        }

        private async Task ValidarVoluntario(Voluntario voluntario)
        {
            await ValidarEntidade(new VoluntarioValidation(), voluntario);
        }

        private void CertificadoValido(Voluntario voluntario)
        {
            if (voluntario.ValidoAte.Date < DateTime.Now.Date)
                NotificarErro("Certificado vencido.");
        }

        private async Task VoluntarioExiste(Guid instituicaoId)
        {
            if (!await _voluntarioRepository.ExisteNaInstituicao(instituicaoId))
                NotificarErro("Voluntário não encontrado.");
        }

        private async Task<bool> VoluntarioAptoParaCadastrar(Voluntario voluntario)
        {
            await ValidarVoluntario(voluntario);
            CertificadoValido(voluntario);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(CadastrarVoluntarioViewModel cadastrarVoluntarioViewModel)
        {
            var dataByteArray = Convert.FromBase64String(cadastrarVoluntarioViewModel.Upload);

            X509Certificate2 certificado;

            try
            {
                certificado = new X509Certificate2(dataByteArray, cadastrarVoluntarioViewModel.Senha);
            }
            catch (CryptographicException)
            {
                return NotificarErro("Senha incorreta.");
            }
            catch (Exception)
            {
                return NotificarErro("Erro desconhecido. Tente novamente");
            }

            if (!certificado.Verify())
                return NotificarErro("Certificado inválido.");

            var pepper = Encoding.UTF8.GetBytes(_appConfig.SecrectKey);

            var senhaCripto = Encrypt(cadastrarVoluntarioViewModel.Senha, InstituicaoId.ToString(), pepper);

            var voluntario = new Voluntario(Guid.NewGuid(), InstituicaoId, ExtrairNomeDoCommonName(certificado.Subject),
                new CnpjCpf(TipoPessoa.Fisica, ExtrairCpnjCpfDoCommonName(certificado.Subject)),
                ExtrairCommonName(certificado.Subject), certificado.NotBefore, certificado.NotAfter,
                ExtrairCommonName(certificado.Issuer), dataByteArray, senhaCripto);

            if (!await VoluntarioAptoParaCadastrar(voluntario))
                return false;

            if (await _voluntarioRepository.ExisteNaInstituicao(InstituicaoId))
                await _voluntarioRepository.Excluir(InstituicaoId);

            await _voluntarioRepository.Cadastrar(voluntario);

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
            var cn = ExtrairCommonName(texto);

            if (string.IsNullOrEmpty(cn))
                return string.Empty;

            var split = cn.Split(":");

            return split.LastOrDefault();
        }

        private async Task<bool> VoluntarioAptoParaExcluir(Guid instituicaoId)
        {
            await VoluntarioExiste(instituicaoId);

            return CommandEhValido();
        }

        public async Task<bool> Excluir(Guid instituicaoId)
        {
            if (!await VoluntarioAptoParaExcluir(instituicaoId))
                return false;

            await _voluntarioRepository.Excluir(instituicaoId);

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
