using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.ConsoleApp.Application.CertificadoDigital;
using Parametriz.AutoNFP.ConsoleApp.Application.CuponsFiscais;
using Parametriz.AutoNFP.ConsoleApp.Application.Docker;
using Parametriz.AutoNFP.ConsoleApp.Application.FileSistem;
using Parametriz.AutoNFP.Core.Configs;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.EnviarCuponsFiscais
{
    public class EnviarCuponsFiscaisService : BaseService, IEnviarCuponsFiscaisService
    {
        private readonly int _keySize = 256;
        private readonly int _ivSize = 128;
        private readonly int _saltSize = 16;

        private readonly AppConfig _appConfig;
        private readonly ICupomFiscalRepository _cupomFiscalRepository;
        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly ICertificadoDigitalService _certificadoDigitalService;
        private readonly IFileSystemService _fileSystemService;
        private readonly IDockerService _dockerService;

        public EnviarCuponsFiscaisService(IUnitOfWork uow,
                                          Notificador notificador,
                                          IOptions<AppConfig> options,
                                          ICupomFiscalRepository cupomFiscalRepository,
                                          IVoluntarioRepository voluntarioRepository,
                                          ICertificadoDigitalService certificadoDigitalService,
                                          IFileSystemService fileSystemService,
                                          IDockerService dockerService)
            : base(uow, notificador)
        {
            _appConfig = options.Value;
            _cupomFiscalRepository = cupomFiscalRepository;
            _voluntarioRepository = voluntarioRepository;
            _certificadoDigitalService = certificadoDigitalService;
            _fileSystemService = fileSystemService;
            _dockerService = dockerService;
        }

        public void ExecutarProcesso()
        {
            var instituicoesId = _cupomFiscalRepository.ObterInstituicoesIdComCuponsFiscaisProcessando();

            if (!int.TryParse(Environment.GetEnvironmentVariable("PORTA_INICIAL_SELENIUM"), out int port))
               port = 4444;

            port--;

            foreach (var instituicaoId in instituicoesId)
            {
                port++;

                var cuponsFiscais = _cupomFiscalRepository.ObterPorStatusProcessando(instituicaoId);
                if (cuponsFiscais.Count() <= 0)
                    continue;

                var voluntario = _voluntarioRepository.ObterPorInstituicaoId(instituicaoId);
                if (voluntario == null)
                    continue; // ToDo: O que fazer?

                var senha = ObterSenha(voluntario);

                var diretorio = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.autonfp/{voluntario.InstituicaoId}";

                var containerName = $"selenium-chrome-{voluntario.InstituicaoId}";
                var imageName = $"{containerName}:latest";

                if (!_certificadoDigitalService.EstaValido(voluntario, senha))
                    continue;

                if (!_fileSystemService.ExecutarProcessoInicial(diretorio, voluntario, senha))
                    continue;

                if (!_dockerService.ExecutarProcessoInicial(diretorio, imageName, containerName, port))
                    continue;



                _fileSystemService.ExecutarProcessoFinal(diretorio);
                _dockerService.ExecutarProcessoFinal(imageName, containerName);
            }
        }

        #region Support
        private string ObterSenha(Voluntario voluntario)
        {
            var pepper = Encoding.UTF8.GetBytes(_appConfig.SecrectKey);
            return Decrypt(voluntario.Senha, voluntario.InstituicaoId.ToString(), pepper);
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
        #endregion Support
    }
}
