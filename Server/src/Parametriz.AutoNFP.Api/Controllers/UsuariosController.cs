using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.Identidade.Services;
using Parametriz.AutoNFP.Api.Application.Usuarios.Services;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UsuarioViewModel>> Get(Guid id)
        {
            var usuarioViewModel = (await _usuarioRepository.ObterPorId(id, InstituicaoId)).ToViewModel();

            if (usuarioViewModel == null)
                return NotFound();

            return usuarioViewModel;
        }

        [HttpGet("obter-por-filtros")]
        public async Task<IEnumerable<UsuarioViewModel>> Get(string nome = "")
        {
            return (await _usuarioRepository.ObterPorFiltros(InstituicaoId, nome)).ToViewModel();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(usuarioViewModel);

            await _identidadeService.CadastrarUsuario(usuarioViewModel);

            return CustomResponse(usuarioViewModel);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _usuarioService.Atualizar(usuarioViewModel);

            return CustomResponse(usuarioViewModel);
        }

        [HttpPut("desativar")]
        public async Task<IActionResult> Desativar([FromBody] UsuarioViewModel usuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _usuarioService.Desativar(usuarioViewModel.Id);

            return CustomResponse(usuarioViewModel);
        }

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
