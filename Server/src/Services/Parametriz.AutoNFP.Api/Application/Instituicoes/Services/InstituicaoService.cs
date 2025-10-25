using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Instituicoes;
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

        private async Task ValidarNovaInstituicao(Instituicao instituicao)
        {
            await ValidarEntidade(new InstituicaoValidation(), instituicao);
            await ValidarEntidade(new CnpjCpfObrigatorioValidation(), instituicao.Cnpj);
            await ValidarEntidade(new VoluntarioValidation(), instituicao.Voluntarios.First());
            await ValidarEntidade(new EmailValidation(), instituicao.Voluntarios.First().Email);
        }

        private async Task ValidarInstituicao(Instituicao instituicao)
        {
            await ValidarEntidade(new InstituicaoValidation(), instituicao);
        }

        private async Task InstituicaoEhUnica(Instituicao instituicao)
        {
            if (!await _instituicaoRepository.EhUnico(instituicao))
                NotificarErro("Instituição já cadastrada.");
        }

        private async Task<bool> InstituicaoAptaParaCadastrar(Instituicao instituicao)
        {
            await ValidarNovaInstituicao(instituicao);
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

            await _instituicaoRepository.CadastrarAsync(instituicao);

            await Commit();
               
            return CommandEhValido();
        }

        private async Task<bool> IntituicaoAptaParaAtualizar(Instituicao instituicao)
        {
            await ValidarInstituicao(instituicao);
            await InstituicaoEhUnica(instituicao);

            return CommandEhValido();
        }

        public async Task<bool> Atualizar(InstituicaoViewModel instituicaoViewModel)
        {
            if (instituicaoViewModel.Id != InstituicaoId)
                return NotificarErro("Requisição inválida.");

            var instituicao = await _instituicaoRepository.ObterPorId(InstituicaoId);

            if (instituicao == null)
                return NotificarErro("Instituição não encontrada.");

            instituicao.AlterarRazaoSocial(instituicaoViewModel.RazaoSocial);
            instituicao.AlterarEntidadeNomeNFP(instituicaoViewModel.EntidadeNomeNFP);
            instituicao.AlterarEndereco(instituicaoViewModel.Endereco.ToDomain());

            if (!await IntituicaoAptaParaAtualizar(instituicao))
                return false;

            _instituicaoRepository.Atualizar(instituicao);

            await Commit();

            return CommandEhValido();
        }

        public async Task<bool> Desativar(Guid id)
        {
            var instituicao = await _instituicaoRepository.ObterPorId(id);

            if (instituicao == null)
                return NotificarErro("Instituição não encontrada.");

            if (instituicao.Desativada)
                return NotificarErro("Instituição não está ativa.");

            instituicao.Desativar();

            _instituicaoRepository.Atualizar(instituicao);

            await Commit();

            return CommandEhValido();
        }

        public async Task<bool> Ativar(Guid id)
        {
            var instituicao = await _instituicaoRepository.ObterPorId(id);

            if (instituicao == null)
                return NotificarErro("Instituição não encontrada.");

            if (!instituicao.Desativada)
                return NotificarErro("Instituição não está desativada.");

            instituicao.Ativar();

            _instituicaoRepository.Atualizar(instituicao);

            await Commit();

            return CommandEhValido();
        }
    }
}
