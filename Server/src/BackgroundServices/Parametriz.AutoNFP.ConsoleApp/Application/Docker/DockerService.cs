using Docker.DotNet;
using Docker.DotNet.Models;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.Docker
{
    public class DockerService : BaseService, IDockerService
    {
        private readonly DockerClient _dockerClient;

        public DockerService(IUnitOfWork uow, 
                             Notificador notificador) 
            : base(uow, notificador)
        {
            var dockerUri = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? 
                "npipe://./pipe/docker_engine" : 
                "unix:/var/run/docker.sock";

            //"tcp://192.168.0.14:2375"
            var uri = new Uri(dockerUri);

            _dockerClient = new DockerClientConfiguration(uri)
                .CreateClient();
        }

        public bool ExecutarProcessoInicial(string diretorio, string imageName, string containerName, int port)
        {
            var imageId = string.Empty;
            var containerId = string.Empty;

            try
            {
                CriarImagem(diretorio, imageName).Wait();
                imageId = ObterImage(imageName).Result;

                containerId = ObterContainer(containerName).Result;
                
                if (!string.IsNullOrEmpty(containerId))
                    ExcluirContainer(containerId).Wait();

                containerId = CriarContainer(imageName, containerName, port).Result;
                
                IniciarContainer(containerId).Wait();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                ExcluirImagem(imageName).Wait();
                ExcluirContainer(containerId).Wait();
            }
        }

        public void ExecutarProcessoFinal(string imageName, string containerName)
        {
            var containerId = ObterContainer(containerName).Result;

            if (!string.IsNullOrEmpty(containerId))
                ExcluirContainer(containerId).Wait();

            var imageId = ObterImage(imageName).Result;

            if (!string.IsNullOrEmpty(imageId))
                ExcluirImagem(imageName).Wait();
        }

        #region Images
        private async Task<string> ObterImage(string imageName)
        {
            var parameters = new ImagesListParameters
            {
                All = true,
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    {
                        "name",
                        new Dictionary<string, bool>
                        {
                            { imageName, true }
                        }
                    }
                }
            };

            var images = await _dockerClient.Images.ListImagesAsync(parameters);

            return images.FirstOrDefault()?.ID;
        }

        private async Task CriarImagem(string diretorio, string imageName)
        {
            using (var tarStream = new MemoryStream())
            {
                TarFile.CreateFromDirectory(diretorio, tarStream, includeBaseDirectory: false);
                tarStream.Seek(0, SeekOrigin.Begin);

                var parameters = new ImageBuildParameters
                {
                    Dockerfile = "./Dockerfile",
                    ShmSize = 2024,
                    Tags = new List<string> { imageName }
                };

                var progress = new Progress<JSONMessage>();
                progress.ProgressChanged += (sender, message) =>
                {
                    Console.WriteLine(message.Stream);
                };

                await _dockerClient.Images.BuildImageFromDockerfileAsync(parameters, tarStream, null, null, progress);
            };
        }

        private async Task ExcluirImagem(string imageName)
        {
            var parameters = new ImageDeleteParameters
            {
                Force = true,
                NoPrune = false
            };

            await _dockerClient.Images.DeleteImageAsync(imageName, parameters);
        }
        #endregion Images

        #region Containers
        private async Task<string> ObterContainer(string containerName)
        {
            var parameters = new ContainersListParameters
            {
                All = true,
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    { 
                        "name", 
                        new Dictionary<string, bool>
                        {
                            { containerName, true }
                        }
                    }
                }
            };
            
            var containers = await _dockerClient.Containers.ListContainersAsync(parameters);

            return containers.FirstOrDefault()?.ID;
        }

        private async Task<string> CriarContainer(string imageName, string containerName, int port)
        {
            var parameters = new CreateContainerParameters()
            {
                Image = imageName,
                Name = containerName,
                ExposedPorts = new Dictionary<string, EmptyStruct>()
                {
                    { port.ToString(), default(EmptyStruct) },
                    { (port + 3456).ToString(), default(EmptyStruct) }
                },
                HostConfig = new HostConfig()
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { port.ToString(), new List<PortBinding> { new PortBinding {  HostPort = port.ToString() } } },
                        { (port + 3456).ToString(), new List<PortBinding>{ new PortBinding { HostPort = (port + 3456).ToString() } } }
                    },
                    RestartPolicy = new RestartPolicy { Name = RestartPolicyKind.No }
                }
            };

            var response = await _dockerClient.Containers.CreateContainerAsync(parameters);

            return response.ID;
        }

        private async Task IniciarContainer(string containerId)
        {
            if (string.IsNullOrEmpty(containerId))
                return;

            await _dockerClient.Containers.StartContainerAsync(containerId, null);
        }

        private async Task ExcluirContainer(string containerId)
        {
            var parameters = new ContainerRemoveParameters
            {
                Force = true
            };

            await _dockerClient.Containers.RemoveContainerAsync(containerId, parameters);
        }
        #endregion Containers
    }
}
