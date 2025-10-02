using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Services
{
    public interface IVoluntarioService
    {
        Task<bool> Cadastrar(Voluntario voluntario);
    }
}
