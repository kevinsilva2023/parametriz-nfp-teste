using Microsoft.AspNetCore.Identity;
using Parametriz.AutoNFP.Api.Application.Certificados.Services;
using Parametriz.AutoNFP.Api.Application.Identidade.Services;
using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Data.Migrations;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.Usuarios;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Services
{
    public class VoluntarioService : BaseService, IVoluntarioService
    {
        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICertificadoRepository _certificadoRepository;

        public VoluntarioService(IAspNetUser user,
                                 IUnitOfWork uow,
                                 Notificador notificador,
                                 UserManager<IdentityUser> userManager,
                                 IVoluntarioRepository voluntarioRepository,
                                 ICertificadoRepository certificadoRepository)
            : base(user, uow, notificador)
        {
            _voluntarioRepository = voluntarioRepository;
            _userManager = userManager;
            _certificadoRepository = certificadoRepository;
        }

        private async Task ValidarVoluntario(Voluntario voluntario)
        {
            await ValidarEntidade(new VoluntarioValidation(), voluntario);
        }

        private async Task VoluntarioEhUnico(Voluntario voluntario)
        {
            if (!await _voluntarioRepository.EhUnico(voluntario))
                _notificador.IncluirNotificacao("Nome, E-mail ou CPF já cadastrado.");
        }

        private async Task ExistemOutrosVoluntariosNaInstituicao(Guid voluntarioId)
        {
            if (!await _voluntarioRepository.ExistemOutrosVoluntariosNaInstituicao(voluntarioId, InstituicaoId))
                NotificarErro("Não foram encontrados outros voluntários na instituição.");
        }

        private async Task ExistemOutrosAdministradoresNaInstituicao(Guid voluntarioId)
        {
            if (!await _voluntarioRepository.ExistemOutrosAdministradoresNaInstituicao(voluntarioId, InstituicaoId))
                NotificarErro("Não foram encontrados outros administradores na instituição.");
        }

        private async Task<bool> VoluntarioAptoParaCadastrar(Voluntario voluntario)
        {
            await ValidarVoluntario(voluntario);
            await VoluntarioEhUnico(voluntario);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(VoluntarioViewModel voluntarioViewModel, Guid id)
        {
            var voluntario = new Voluntario(id, InstituicaoId, voluntarioViewModel.Nome, voluntarioViewModel.Cpf,
                voluntarioViewModel.Email, voluntarioViewModel.Contato, voluntarioViewModel.Administrador);

            if (!await VoluntarioAptoParaCadastrar(voluntario))
                return false;

            await _voluntarioRepository.Cadastrar(voluntario);

            await Commit();

            return CommandEhValido();
        }

        private async Task<bool> VoluntarioAptoParaAtualizar(Voluntario voluntario)
        {
            await ValidarVoluntario(voluntario);
            await VoluntarioEhUnico(voluntario);

            if (!voluntario.Administrador)
                await ExistemOutrosAdministradoresNaInstituicao(voluntario.Id);

            return CommandEhValido();
        }

        public async Task<bool> AtualizarPerfil(VoluntarioViewModel voluntarioViewModel)
        {
            if (VoluntarioId != voluntarioViewModel.Id)
                return NotificarErro("Requisição inválida.");

            var voluntario = await _voluntarioRepository.ObterPorId(voluntarioViewModel.Id, InstituicaoId);

            if (voluntario == null)
                return NotificarErro("Voluntário não encontrado.");
            
            voluntario.AlterarNome(voluntarioViewModel.Nome);
            voluntario.AlterarContato(voluntarioViewModel.Contato);
            voluntario.AlterarFotoUpload(voluntarioViewModel.FotoUpload);
            
            if (!await VoluntarioAptoParaAtualizar(voluntario))
                return false;

            _voluntarioRepository.Atualizar(voluntario);

            await Commit();

            return CommandEhValido();
        }

        public async Task<bool> Atualizar(VoluntarioViewModel voluntarioViewModel)
        {
            var voluntario = await _voluntarioRepository.ObterPorId(voluntarioViewModel.Id, InstituicaoId);

            if (voluntario == null)
                return NotificarErro("Voluntário não encontrado.");
            
            var eraAdministrador = voluntario.Administrador;

            voluntario.AlterarAdministrador(voluntarioViewModel.Administrador);

            if (!await VoluntarioAptoParaAtualizar(voluntario))
                return false;

            _voluntarioRepository.Atualizar(voluntario);

            var resultRole = true;
            if (eraAdministrador && !voluntario.Administrador)
                resultRole = await RemoverRoleAdministradorDoVoluntario(voluntario.Id);

            if (!eraAdministrador && voluntario.Administrador)
                resultRole = await AdicionarRoleAdministradorDoVoluntario(voluntario.Id);

            if (resultRole)
                await Commit();

            return CommandEhValido();
        }

        private async Task<bool> VoluntarioAptoParaDesativar(Guid voluntarioId)
        {
            await ExistemOutrosVoluntariosNaInstituicao(voluntarioId);
            await ExistemOutrosAdministradoresNaInstituicao(voluntarioId);
            
            return CommandEhValido();
        }

        public async Task<bool> Desativar(Guid id)
        {
            var voluntario = await _voluntarioRepository.ObterPorId(id, InstituicaoId);

            if (voluntario == null)
                NotificarErro("Voluntário não encontrado.");

            if (voluntario.Desativado)
                NotificarErro("Voluntário não está ativo.");

            var eraAdministrador = voluntario.Administrador;

            if (!await VoluntarioAptoParaDesativar(voluntario.Id))
                return false;

            voluntario.Desativar();

            _voluntarioRepository.Atualizar(voluntario);
            
            if (voluntario.Certificado != null)
                _certificadoRepository.Excluir(voluntario.Certificado);

            var resultRole = true;
            if (eraAdministrador)
                resultRole = await RemoverRoleAdministradorDoVoluntario(voluntario.Id);

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
            var voluntario = await _voluntarioRepository.ObterPorId(id, InstituicaoId);

            if (voluntario == null)
                NotificarErro("Voluntário não encontrado.");

            if (!voluntario.Desativado)
                NotificarErro("Voluntário não está desativado.");

            //if (!await UsuarioAptoParaAtivar(usuario))
            //    return false;

            voluntario.Ativar();

            _voluntarioRepository.Atualizar(voluntario);

            await Commit();

            return CommandEhValido();
        }

        #region Identity
        private async Task<bool> AdicionarRoleAdministradorDoVoluntario(Guid usuarioId)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());

            if (user == null)
                return false;

            var result = await _userManager.AddToRoleAsync(user, "Administrador");

            if (!result.Succeeded)
                return AdicionarErrosIdentity(result);

            return true;
        }

        private async Task<bool> RemoverRoleAdministradorDoVoluntario(Guid usuarioId)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());

            if (user == null)
                return false;

            var result = await _userManager.RemoveFromRoleAsync(user, "Administrador");

            if (!result.Succeeded)
                return AdicionarErrosIdentity(result);

            await _userManager.UpdateSecurityStampAsync(user);

            return true;
        }
        #endregion Identity
    }
}
