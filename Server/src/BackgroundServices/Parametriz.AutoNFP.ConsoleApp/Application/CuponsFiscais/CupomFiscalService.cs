using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.CuponsFiscais
{
    public class CupomFiscalService : BaseService, ICupomFiscalService
    {
        private readonly ICupomFiscalRepository _cupomFiscalRepository;

        public CupomFiscalService(IUnitOfWork uow,
                                  Notificador notificador,
                                  ICupomFiscalRepository cupomFiscalRepository)
            : base(uow, notificador)
        {
            _cupomFiscalRepository = cupomFiscalRepository;
        }
    }
}
