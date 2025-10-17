using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.ConsoleApp.Application.FileSistem;
using Parametriz.AutoNFP.Core.Configs;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.CertificadoDigital
{
    public class CertificadoDigitalService : BaseService, ICertificadoDigitalService
    {
        private readonly int _keySize = 256;
        private readonly int _ivSize = 128;
        private readonly int _saltSize = 16;

        private readonly AppConfig _appConfig;
        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly IFileSystemService _fileSystemService;

        public CertificadoDigitalService(IUnitOfWork uow, 
                                         Notificador notificador,
                                         IOptions<AppConfig> options) 
            : base(uow, notificador)
        {

            _appConfig = options.Value;
        }

        public bool ExecutarProcesso(Guid instituicaoId)
        {
            var voluntario = _voluntarioRepository.ObterPorInstituicaoId(instituicaoId);

            var pepper = Encoding.UTF8.GetBytes(_appConfig.SecrectKey);
            var senha = Decrypt(voluntario.Senha, instituicaoId.ToString(), pepper);

            var certificado = new X509Certificate2(voluntario.Upload, senha);

            if (certificado == null || !certificado.Verify())
                return false; // ToDo: O que fazer quando certificado não está valido?

            if (!_fileSystemService.ExecutarProcesso(voluntario, senha))
                return false;

            return true;
        }

        private string Decrypt(byte[] encryptedBytes, string password, byte[] pepper)
        {
            var salt = new byte[_saltSize];
            var iv = new byte[_ivSize / 8];
            var cipherText = new byte[encryptedBytes.Length - _saltSize - iv.Length];

            Buffer.BlockCopy(encryptedBytes, 0, salt, 0, _saltSize);
            Buffer.BlockCopy(encryptedBytes, _saltSize, iv, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, _saltSize + iv.Length, cipherText, 0, cipherText.Length);

            var key = DeriveKey(password, salt, pepper);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var memoryStream = new MemoryStream(cipherText))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
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


    }
}
