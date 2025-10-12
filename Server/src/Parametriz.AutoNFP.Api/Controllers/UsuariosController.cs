using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.Identidade.Services;
using Parametriz.AutoNFP.Api.Application.Usuarios.Services;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Domain.Core.Enums;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/usuarios")]
    public class UsuariosController : MainController
    {
        private readonly IIdentidadeService _identidadeService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioService _usuarioService;
        public UsuariosController(Notificador notificador,
                                  IAspNetUser user,
                                  IUsuarioRepository usuarioRepository,
                                  IUsuarioService usuarioService,
                                  IIdentidadeService identidadeService)
            : base(notificador, user)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _identidadeService = identidadeService;
        }

        [HttpGet("nao-administrador")]
        public async Task<ActionResult<UsuarioViewModel>> Get()
        {
            return (await _usuarioRepository.ObterPorId(UsuarioId, InstituicaoId))
                .ToViewModel();
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UsuarioViewModel>> Get(Guid id)
        {
            var usuarioViewModel = (await _usuarioRepository.ObterPorId(id, InstituicaoId)).ToViewModel();

            if (usuarioViewModel == null)
                return NotFound();

            return usuarioViewModel;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("obter-por-filtros")]
        public async Task<IEnumerable<UsuarioViewModel>> Get(string nome = "", string email = "", 
            BoolTresEstados administrador = BoolTresEstados.Ambos, BoolTresEstados desativado = BoolTresEstados.Falso)
        {
            return (await _usuarioRepository
                .ObterPorFiltros(InstituicaoId, nome, email, administrador, desativado))
                .ToViewModel();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(usuarioViewModel);

            await _identidadeService.CadastrarUsuario(usuarioViewModel);

            return CustomResponse(usuarioViewModel);
        }

        [HttpPut("nao-administrador")]
        public async Task<IActionResult> AtualizarNaoAdministrador([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _usuarioService.AtualizarNaoAdministrador(usuarioViewModel);

            return CustomResponse(usuarioViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _usuarioService.Atualizar(usuarioViewModel);

            return CustomResponse(usuarioViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("desativar")]
        public async Task<IActionResult> Desativar([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _usuarioService.Desativar(usuarioViewModel.Id);

            return CustomResponse(usuarioViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("ativar")]
        public async Task<IActionResult> Ativar([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _usuarioService.Ativar(usuarioViewModel.Id);

            return CustomResponse(usuarioViewModel);
        }
    }
}
