using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.Usuarios.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IAspNetUser user,
                                 IUnitOfWork uow,
                                 Notificador notificador,
                                 IUsuarioRepository usuarioRepository)
            : base(user, uow, notificador)
        {
            _usuarioRepository = usuarioRepository;
        }

        private async Task ValidarUsuario(Usuario usuario)
        {
            await ValidarEntidade(new UsuarioValidation(), usuario);
        }

        private async Task UsuarioEhUnico(Usuario usuario)
        {
            if (!await _usuarioRepository.EhUnico(usuario))
                _notificador.IncluirNotificacao("Usuário já cadastrado.");
        }

        private async Task<bool> UsuarioAptoParaCadastrar(Usuario usuario)
        {
            ValidarInstituicao(usuario.InstituicaoId);
            await ValidarUsuario(usuario);
            await UsuarioEhUnico(usuario);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(CadastrarUsuarioViewModel cadastrarUsuarioViewModel)
        {
            var usuario = new Usuario(Guid.NewGuid(), cadastrarUsuarioViewModel.InstituicaoId, 
                cadastrarUsuarioViewModel.Nome, new Domain.Core.ValueObjects.Email(cadastrarUsuarioViewModel.Email));

            if (!await UsuarioAptoParaCadastrar(usuario))
                return false;

            await _usuarioRepository.Cadastrar(usuario);

            await Commit();

            return CommandEhValido();
        }
    }
}
