using Parametriz.AutoNFP.Domain.CuponsFiscais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.CuponsFiscais
{
    public interface ICupomFiscalService
    {
        void Atualizar(CupomFiscal cupomFiscal);
    }
}
