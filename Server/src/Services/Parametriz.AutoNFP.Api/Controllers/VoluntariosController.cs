using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.Identidade.Services;
using Parametriz.AutoNFP.Api.Application.Voluntarios.Queries;
using Parametriz.AutoNFP.Api.Application.Voluntarios.Services;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/voluntarios")]
    public class VoluntariosController : MainController
    {
        private readonly IIdentidadeService _identidadeService;
        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly IVoluntarioService _voluntarioService;
        private readonly IVoluntarioQuery _voluntarioQuery;

        public VoluntariosController(Notificador notificador,
                                  IAspNetUser user,
                                  IVoluntarioRepository voluntarioRepository,
                                  IVoluntarioService voluntarioService,
                                  IIdentidadeService identidadeService,
                                  IVoluntarioQuery voluntarioQuery)
            : base(notificador, user)
        {
            _voluntarioRepository = voluntarioRepository;
            _voluntarioService = voluntarioService;
            _identidadeService = identidadeService;
            _voluntarioQuery = voluntarioQuery;
        }

        [HttpGet("perfil")]
        public async Task<ActionResult<VoluntarioViewModel>> Get()
        {
            return (await _voluntarioRepository.ObterPorId(VoluntarioId, InstituicaoId))
                .ToViewModel();
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VoluntarioViewModel>> Get(Guid id)
        {
            var voluntarioViewModel = (await _voluntarioRepository.ObterPorId(id, InstituicaoId)).ToViewModel();

            if (voluntarioViewModel == null)
                return NotFound();

            return voluntarioViewModel;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("obter-por-filtros")]
        public async Task<IEnumerable<VoluntarioViewModel>> Get(string nome = "", string email = "", 
            CertificadoStatus? certificadoStatus = null, BoolTresEstados administrador = BoolTresEstados.Ambos, 
            BoolTresEstados desativado = BoolTresEstados.Falso)
        {
            return await _voluntarioQuery
                .ObterPorFiltros(InstituicaoId, nome, email, certificadoStatus, administrador, desativado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("obter-ativos")]
        public async Task<IEnumerable<VoluntarioViewModel>> ObterAtivos()
        {
            return (await _voluntarioRepository.ObterAtivos(InstituicaoId))
                .ToViewModel();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VoluntarioViewModel voluntarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(voluntarioViewModel);

            await _identidadeService.CadastrarVoluntario(voluntarioViewModel);

            return CustomResponse(voluntarioViewModel);
        }

        [HttpPut("perfil")]
        public async Task<IActionResult> AtualizarPerfil([FromBody] VoluntarioViewModel voluntarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            if (!string.IsNullOrWhiteSpace(voluntarioViewModel.FotoUpload))
            {
                var limiteUpload = 240 * 1024; // 240Kb. (No domain o limite é 200Kb por que na string base64 o calculo é aproximado)
                var tamanhoAproximadoUpload = voluntarioViewModel.FotoUpload.Length * 0.75; // Convertido possui aproximadamente 1.37% 4/3

                if (tamanhoAproximadoUpload > limiteUpload)
                {
                    NotificarErro("Foto excede o limite do tamanho permitido.");
                    return CustomResponse(voluntarioViewModel);
                } 
            }

            await _voluntarioService.AtualizarPerfil(voluntarioViewModel);

            return CustomResponse(voluntarioViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] VoluntarioViewModel voluntarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _voluntarioService.Atualizar(voluntarioViewModel);

            return CustomResponse(voluntarioViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("desativar")]
        public async Task<IActionResult> Desativar([FromBody] VoluntarioViewModel voluntarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _voluntarioService.Desativar(voluntarioViewModel.Id);

            return CustomResponse(voluntarioViewModel);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("ativar")]
        public async Task<IActionResult> Ativar([FromBody] VoluntarioViewModel voluntarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _voluntarioService.Ativar(voluntarioViewModel.Id);

            return CustomResponse(voluntarioViewModel);
        }
    }
}
