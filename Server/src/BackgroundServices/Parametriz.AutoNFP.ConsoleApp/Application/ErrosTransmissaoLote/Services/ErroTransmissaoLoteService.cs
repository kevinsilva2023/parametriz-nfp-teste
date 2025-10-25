using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.ErrosTransmissaoLote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.ErrosTransmissaoLote.Services
{
    public class ErroTransmissaoLoteService : BaseService, IErroTransmissaoLoteService
    {
        private readonly IErroTransmissaoLoteRepository _erroTransmissaoLoteRepository;

        public ErroTransmissaoLoteService(IUnitOfWork uow,
                                          Notificador notificador,
                                          IErroTransmissaoLoteRepository erroTransmissaoLoteRepository)
            : base(uow, notificador)
        {
            _erroTransmissaoLoteRepository = erroTransmissaoLoteRepository;
        }

        public bool CadastrarParaInstituicao(Guid instituicaoId, string mensagem)
        {
            var erroTransmissaoLote = new ErroTransmissaoLote(Guid.NewGuid(), instituicaoId, mensagem);

            if (_erroTransmissaoLoteRepository.Existe(instituicaoId, null, mensagem))
                return false;

            _erroTransmissaoLoteRepository.Cadastrar(erroTransmissaoLote);

            Commit();

            return CommandEhValido();
        }

        public bool CadastrarParaVoluntario(Guid instituicaoId, Guid voluntarioId, string mensagem)
        {
            var erroTransmissaoLote = new ErroTransmissaoLote(Guid.NewGuid(), instituicaoId, voluntarioId, mensagem);

            if (_erroTransmissaoLoteRepository.Existe(instituicaoId, voluntarioId, mensagem))
                return false;

            _erroTransmissaoLoteRepository.Cadastrar(erroTransmissaoLote);

            Commit();

            return CommandEhValido();
        }

        public bool ExcluirPorInstituicaoId(Guid instituicaoId)
        {
            var errosTransmissaoLote = _erroTransmissaoLoteRepository.ObterPorInstituicaoId(instituicaoId);

            if (!errosTransmissaoLote.Any())
                return true;

            _erroTransmissaoLoteRepository.ExcluirLista(errosTransmissaoLote);

            Commit();

            return CommandEhValido();
        }
    }
}
