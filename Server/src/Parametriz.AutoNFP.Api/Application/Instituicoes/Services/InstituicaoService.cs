using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.Instituicoes.Services
{
    public class InstituicaoService : BaseService, IInstituicaoService
    {
        private readonly IInstituicaoRepository _instituicaoRepository;

        public InstituicaoService(IAspNetUser user,
                                  IUnitOfWork uow,
                                  Notificador notificador,
                                  IInstituicaoRepository instituicaoRepository)
            : base(user, uow, notificador)
        {
            _instituicaoRepository = instituicaoRepository;
        }

        private async Task ValidarInstituicao(Instituicao instituicao)
        {
            await ValidarEntidade(new InstituicaoValidation(), instituicao);
            await ValidarEntidade(new CnpjCpfObrigatorioValidation(), instituicao.Cnpj);
            await ValidarEntidade(new UsuarioValidation(), instituicao.Usuarios.First());
            await ValidarEntidade(new EmailValidation(), instituicao.Usuarios.First().Email);
        }

        private async Task InstituicaoEhUnica(Instituicao instituicao)
        {
            if (!await _instituicaoRepository.EhUnico(instituicao))
                NotificarErro("Instituição já cadastrada.");
        }

        private async Task<bool> InstituicaoAptaParaCadastrar(Instituicao instituicao)
        {
            await ValidarInstituicao(instituicao);
            await InstituicaoEhUnica(instituicao);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel, Guid usuarioId)
        {
            var instituicao = new Instituicao(cadastrarInstituicaoViewModel.Id, cadastrarInstituicaoViewModel.RazaoSocial,
                cadastrarInstituicaoViewModel.Cnpj);

            var usuario = new Usuario(usuarioId, instituicao.Id, cadastrarInstituicaoViewModel.UsuarioNome,
                new Domain.Core.ValueObjects.Email(cadastrarInstituicaoViewModel.Email), true);

            instituicao.IncluirUsuario(usuario);

            if (!await InstituicaoAptaParaCadastrar(instituicao))
                return false;

            await _instituicaoRepository.Cadastrar(instituicao);

            await Commit();
               
            return CommandEhValido();
        }
    }
}
