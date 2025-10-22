using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Usuarios;
using Parametriz.AutoNFP.Domain.Voluntarios;

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
            await ValidarEntidade(new VoluntarioValidation(), instituicao.Voluntarios.First());
            await ValidarEntidade(new EmailValidation(), instituicao.Voluntarios.First().Email);
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

        public async Task<bool> Cadastrar(CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel, Guid userId)
        {
            var instituicao = new Instituicao(cadastrarInstituicaoViewModel.Id, cadastrarInstituicaoViewModel.RazaoSocial,
                cadastrarInstituicaoViewModel.Cnpj, cadastrarInstituicaoViewModel.EntidadeNomeNFP, 
                cadastrarInstituicaoViewModel.Endereco.ToDomain());

            var voluntario = new Voluntario(userId, instituicao.Id, cadastrarInstituicaoViewModel.VoluntarioNome,
                cadastrarInstituicaoViewModel.Cpf, cadastrarInstituicaoViewModel.Email, cadastrarInstituicaoViewModel.Contato, 
                true);

            instituicao.IncluirVoluntario(voluntario);

            if (!await InstituicaoAptaParaCadastrar(instituicao))
                return false;

            await _instituicaoRepository.Cadastrar(instituicao);

            await Commit();
               
            return CommandEhValido();
        }
    }
}
