using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.ErrosTransmissaoLote.Services
{
    public interface IErroTransmissaoLoteService
    {
        bool CadastrarParaInstituicao(Guid instituicaoId, string mensagem);
        bool CadastrarParaVoluntario(Guid instituicaoId, Guid voluntarioId, string mensagem);
        bool ExcluirPorInstituicaoId(Guid instituicaoId);
    }
}
