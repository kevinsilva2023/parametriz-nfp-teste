using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _uow;
        protected readonly Notificador _notificador;

        public BaseService(IUnitOfWork uow, 
                           Notificador notificador)
        {
            _uow = uow;
            _notificador = notificador;
        }

        protected bool NotificarErro(string value)
        {
            _notificador.IncluirNotificacao(value);

            return false;
        }

        protected bool CommandEhValido()
        {
            return !_notificador.TemNotificacoes();
        }

        protected virtual bool Commit()
        {
            if (!CommandEhValido())
                return false;

            //if (_uow.Commit())
                return true;

            //return NotificarErro("Ocorreu um erro ao salvar os dados no banco");
        }

    }
}
