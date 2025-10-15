using Microsoft.AspNetCore.Identity;
using Parametriz.AutoNFP.Api.Application.Identidade.Services;
using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Data.Migrations;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.Usuarios.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public UsuarioService(IAspNetUser user,
                              IUnitOfWork uow,
                              Notificador notificador,
                              UserManager<IdentityUser> userManager,
                              IUsuarioRepository usuarioRepository)
            : base(user, uow, notificador)
        {
            _usuarioRepository = usuarioRepository;
            _userManager = userManager;
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

        private async Task ExistemOutrosUsuariosNaInstituicao(Guid usuarioId)
        {
            if (!await _usuarioRepository.ExistemOutrosUsuariosNaInstituicao(usuarioId, InstituicaoId))
                NotificarErro("Não foram encontrados outros usuários na instituição.");
        }

        private async Task ExistemOutrosAdministradoresNaInstituicao(Guid usuarioId)
        {
            if (!await _usuarioRepository.ExistemOutrosAdministradoresNaInstituicao(usuarioId, InstituicaoId))
                NotificarErro("Não foram encontrados outros administradores na instituição.");
        }

        private async Task<bool> UsuarioAptoParaCadastrar(Usuario usuario)
        {
            await ValidarUsuario(usuario);
            await UsuarioEhUnico(usuario);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(UsuarioViewModel usuarioViewModel, Guid usuarioId)
        {
            var usuario = new Usuario(usuarioId, InstituicaoId, usuarioViewModel.Nome, 
                usuarioViewModel.Email.ToDomain(), usuarioViewModel.Administrador);

            if (!await UsuarioAptoParaCadastrar(usuario))
                return false;

            await _usuarioRepository.Cadastrar(usuario);

            await Commit();

            return CommandEhValido();
        }

        private async Task<bool> UsuarioAptoParaAtualizar(Usuario usuario)
        {
            await ValidarUsuario(usuario);
            await UsuarioEhUnico(usuario);

            if (!usuario.Administrador)
                await ExistemOutrosAdministradoresNaInstituicao(usuario.Id);

            return CommandEhValido();
        }

        public async Task<bool> AtualizarNaoAdministrador(UsuarioViewModel usuarioViewModel)
        {
            if (UsuarioId != usuarioViewModel.Id)
                return NotificarErro("Requisição inválida.");

            var usuario = await _usuarioRepository.ObterPorId(usuarioViewModel.Id, InstituicaoId);

            if (usuario == null)
                return NotificarErro("Usuário não encontrado.");

            if (usuario.Administrador)
                return NotificarErro("Usuário é administrador.");

            usuario.AlterarNome(usuarioViewModel.Nome);

            if (!await UsuarioAptoParaAtualizar(usuario))
                return false;

            _usuarioRepository.Atualizar(usuario);

            await Commit();

            return CommandEhValido();
        }

        public async Task<bool> Atualizar(UsuarioViewModel usuarioViewModel)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioViewModel.Id, InstituicaoId);

            if (usuario == null)
                return NotificarErro("Usuário não encontrado.");
            
            var eraAdministrador = usuario.Administrador;

            usuario.AlterarNome(usuarioViewModel.Nome);
            usuario.AlterarAdministrador(usuarioViewModel.Administrador);

            if (!await UsuarioAptoParaAtualizar(usuario))
                return false;

            _usuarioRepository.Atualizar(usuario);

            var resultRole = true;
            if (eraAdministrador && !usuario.Administrador)
                resultRole = await RemoverRoleAdministradorDoUsuario(usuario.Id);

            if (!eraAdministrador && usuario.Administrador)
                resultRole = await AdicionarRoleAdministradorDoUsuario(usuario.Id);

            if (resultRole)
                await Commit();

            return CommandEhValido();
        }

        private async Task<bool> UsuarioAptoParaDesativar(Guid usuarioId)
        {
            await ExistemOutrosUsuariosNaInstituicao(usuarioId);
            await ExistemOutrosAdministradoresNaInstituicao(usuarioId);
            
            return CommandEhValido();
        }

        public async Task<bool> Desativar(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id, InstituicaoId);

            if (usuario == null)
                NotificarErro("Usuário não encontrado.");

            if (usuario.Desativado)
                NotificarErro("Usuário não está ativo.");

            var eraAdministrador = usuario.Administrador;

            if (!await UsuarioAptoParaDesativar(usuario.Id))
                return false;

            usuario.Desativar();

            _usuarioRepository.Atualizar(usuario);

            var resultRole = true;
            if (eraAdministrador)
                resultRole = await RemoverRoleAdministradorDoUsuario(usuario.Id);

            if (resultRole)
                await Commit();

            return CommandEhValido();
        }

        //private async Task<bool> UsuarioAptoParaAtivar(Usuario usuario)
        //{
        //    return CommandEhValido();
        //}

        public async Task<bool> Ativar(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id, InstituicaoId);

            if (usuario == null)
                NotificarErro("Usuário não encontrado.");

            if (!usuario.Desativado)
                NotificarErro("Usuário não está desativado.");

            //if (!await UsuarioAptoParaAtivar(usuario))
            //    return false;

            usuario.Ativar();

            _usuarioRepository.Atualizar(usuario);

            await Commit();

            return CommandEhValido();
        }

        #region Identity


        private async Task<bool> AdicionarRoleAdministradorDoUsuario(Guid usuarioId)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());

            if (user == null)
                return false;

            var result = await _userManager.AddToRoleAsync(user, "Administrador");

            if (!result.Succeeded)
                return AdicionarErrosIdentity(result);

            return true;
        }

        private async Task<bool> RemoverRoleAdministradorDoUsuario(Guid usuarioId)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());

            if (user == null)
                return false;

            var result = await _userManager.RemoveFromRoleAsync(user, "Administrador");

            if (!result.Succeeded)
                return AdicionarErrosIdentity(result);

            return true;
        }
        #endregion Identity
    }
}
