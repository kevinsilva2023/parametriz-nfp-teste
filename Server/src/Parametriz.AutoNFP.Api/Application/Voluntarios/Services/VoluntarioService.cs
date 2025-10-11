using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.Api.Configs;
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
        private readonly int _ivSize = 16; // 128 bits para o IV

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

        private async Task CertificadoValido(Voluntario voluntario)
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
            await CertificadoValido(voluntario);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(CadastrarVoluntarioViewModel cadastrarVoluntarioViewModel)
        {
            var dataByteArray = Convert.FromBase64String(cadastrarVoluntarioViewModel.Upload);

            var certificado = new X509Certificate2(dataByteArray, cadastrarVoluntarioViewModel.Senha);

            var voluntario = new Voluntario(Guid.NewGuid(), InstituicaoId, ExtrairNomeDoCommonName(certificado.Subject), 
                new CnpjCpf(TipoPessoa.Fisica, ExtrairCpnjCpfDoCommonName(certificado.Subject)), 
                ExtrairCommonName(certificado.Subject), certificado.NotBefore, certificado.NotAfter,
                ExtrairCommonName(certificado.Issuer), dataByteArray, Encrypt(cadastrarVoluntarioViewModel.Senha));

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

        

        //// Este método é para demonstração. Em produção, recupere a chave de uma fonte segura.
        //private byte[] GetKey()
        //{
        //    // Use uma chave segura e gerenciada. Por exemplo, do Azure Key Vault.
        //    // Nunca armazene a chave em texto plano no código ou arquivos de configuração.
        //    return Encoding.UTF8.GetBytes(_appConfig.SecrectKey); // Exemplo
        //}

        /// <summary>
        /// Criptografa uma string usando AES e retorna uma string em Base64.
        /// Inclui o IV no início da string criptografada para facilitar a descriptografia.
        /// </summary>
        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            
            aes.KeySize = _keySize;
            aes.Mode = CipherMode.CBC; // CBC é um modo seguro.

            aes.Key = Encoding.UTF8.GetBytes(_appConfig.SecrectKey);
            aes.GenerateIV(); // Cria um IV único para cada operação.

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            
            // Escreve o IV na stream para recuperá-lo na descriptografia.
            memoryStream.Write(aes.IV, 0, _ivSize);

            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            using var streamWriter = new StreamWriter(cryptoStream);
                
            streamWriter.Write(plainText);
            
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// Descriptografa uma string em Base64 usando AES.
        /// </summary>
        public string Decrypt(string cipherTextBase64)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherTextBase64);

            using var aes = Aes.Create();
            
            aes.KeySize = _keySize;
            aes.Mode = CipherMode.CBC;

            aes.Key = Encoding.UTF8.GetBytes(_appConfig.SecrectKey);

            byte[] iv = new byte[_ivSize];
            Array.Copy(fullCipher, 0, iv, 0, _ivSize);
            aes.IV = iv;

            using var memoryStream = new MemoryStream();
            
            // Copia o texto cifrado (sem o IV) para a stream.
            memoryStream.Write(fullCipher, _ivSize, fullCipher.Length - _ivSize);
            memoryStream.Seek(0, SeekOrigin.Begin); // Retorna ao início da stream.

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            using var streamREader = new StreamReader(cryptoStream);
           
            return streamREader.ReadToEnd();
        }
    }
}
