using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.FileSistem
{
    public class FileSystemService : BaseService, IFileSystemService
    {
        public FileSystemService(IUnitOfWork uow, 
                                 Notificador notificador) 
            : base(uow, notificador)
        {
        }

        public bool ExecutarProcessoInicial(string diretorio, Certificado certificado, string senha)
        {
            try
            {
                VerificarDiretorioDaInstituicao(diretorio);
                CriarChromePolicy(diretorio, certificado);
                SalvarCertificado(diretorio, certificado);
                CriarDockerfile(diretorio, certificado, senha);
                //CriarDockerCompose(diretorio, voluntario, port);
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ExcluirDiretorioSeExistir(diretorio);
                return false;
            }
        }

        public void ExecutarProcessoFinal(string diretorio)
        {
            ExcluirDiretorioSeExistir(diretorio);
        }

        private void ExcluirDiretorioSeExistir(string diretorio)
        {
            if (Directory.Exists(diretorio))
                Directory.Delete(diretorio, true);
        }

        private void VerificarDiretorioDaInstituicao(string diretorio)
        {
            ExcluirDiretorioSeExistir(diretorio);

            Directory.CreateDirectory(diretorio);
        }

        private void CriarChromePolicy(string diretorio, Certificado certificado)
        {
            var enderecoChromePolicy = Path.Combine(diretorio, "auto_select_certificate.json");

            using var stream = File.CreateText(enderecoChromePolicy);

            stream.Write($@"{{ ""AutoSelectCertificateForUrls"": [ ""{{ \""pattern\"": \""https://[*.]fazenda.sp.gov.br\"", " +
                $@"\""filter\"": {{ \""ISSUER\"": {{ \""CN\"": \""{certificado.Emissor}\"" }}, \""SUBJECT\"": {{ " +
                $@"\""CN\"": \""{certificado.Requerente}\"" }} }} }}"" ] }}");

            stream.Close();
        }

        private void SalvarCertificado(string diretorio, Certificado certificado)
        {
            var enderecoCertificado = Path.Combine(diretorio, "certificado.pfx");
            File.WriteAllBytes(enderecoCertificado, certificado.Upload);
        }

        private void CriarDockerfile(string diretorio, Certificado certificado, string senha)
        {
            var enderecoDockerfile = Path.Combine(diretorio, "Dockerfile");

            using var stream = File.CreateText(enderecoDockerfile);

            #region SemContaNoDocker
            stream.WriteLine($"FROM selenium/standalone-chrome:latest");
            stream.WriteLine($"RUN sudo apt-get update");
            stream.WriteLine($"RUN sudo apt-get install -y libnss3-tools openssl");
            stream.WriteLine($"RUN sudo mkdir -p /etc/opt/chrome/policies/managed");
            #endregion

            #region ComContaNoDockerCompartilhada
            //stream.WriteLine($"maumitsuo/parametriz-autonfp-base-selenium:latest");
            #endregion ComContaNoDockerCompartilhada

            stream.WriteLine($"COPY auto_select_certificate.json /etc/opt/chrome/policies/managed");
            stream.WriteLine($"COPY certificado.pfx /home/seluser");
            stream.WriteLine($"RUN pk12util -d /home/seluser/.pki/nssdb -i /home/seluser/certificado.pfx -W {senha}");

            stream.Close();
        }

        //private void CriarDockerCompose(string diretorio, Voluntario voluntario, int port)
        //{
        //    var enderecoDockerCompose = Path.Combine(diretorio, $"docker-compose.yaml");

        //    using var stream = File.CreateText(enderecoDockerCompose);

        //    stream.WriteLine(@$"services:");
        //    stream.WriteLine($@"    selenium-chrome-{voluntario.InstituicaoId}:");
        //    stream.WriteLine($@"        image: selenium-chrome-{voluntario.InstituicaoId}:latest");
        //    stream.WriteLine(@$"        container_name: selenium-chrome-{voluntario.InstituicaoId}");
        //    stream.WriteLine(@$"        build:");
        //    stream.WriteLine(@$"            context: ./");
        //    stream.WriteLine(@$"            dockerfile: ./Dockerfile");
        //    stream.WriteLine(@$"        shm_size: 2gb");
        //    stream.WriteLine(@$"        ports:");
        //    stream.WriteLine(@$"            - ""{port}:{port}""");
        //    stream.WriteLine(@$"            - ""{port+3456}:{port+3456}""");

        //    stream.Close();
        //}

        
    }
}
