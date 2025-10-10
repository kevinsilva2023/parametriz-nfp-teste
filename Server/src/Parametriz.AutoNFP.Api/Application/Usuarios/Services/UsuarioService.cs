using Microsoft.AspNetCore.Identity;
using Parametriz.AutoNFP.Api.Application.Identidade.Services;
using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.Usuarios.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IIdentidadeService _identidadeService;

        public UsuarioService(IAspNetUser user,
                              IUnitOfWork uow,
                              Notificador notificador,
                              IUsuarioRepository usuarioRepository,
                              IIdentidadeService identidadeService)
            : base(user, uow, notificador)
        {
            _usuarioRepository = usuarioRepository;
            _identidadeService = identidadeService;
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
            ValidarInstituicao(usuario.InstituicaoId);
            await ValidarUsuario(usuario);
            await UsuarioEhUnico(usuario);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(UsuarioViewModel usuarioViewModel, Guid usuarioId)
        {
            var usuario = new Usuario(usuarioId, usuarioViewModel.InstituicaoId, usuarioViewModel.Nome, 
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
                resultRole = await _identidadeService.RemoverRoleDoUsuario(usuario.Id, "Administrador");

            if (!eraAdministrador && usuario.Administrador)
                resultRole = await _identidadeService.CadastrarRoleNoUsuario(usuario.Id, "Administrador");

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
                resultRole = await _identidadeService.RemoverRoleDoUsuario(usuario.Id, "Administrador");

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

            usuario.Desativar();

            _usuarioRepository.Atualizar(usuario);

            await Commit();

            return CommandEhValido();
        }
    }
}
