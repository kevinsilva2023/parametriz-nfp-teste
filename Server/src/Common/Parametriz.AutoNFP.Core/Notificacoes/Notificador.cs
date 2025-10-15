using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Core.Notificacoes
{
    public class Notificador : IDisposable
    {
        private readonly List<string> _notificacoes;
        public IReadOnlyCollection<string> Notificacoes => _notificacoes.AsReadOnly();

        public Notificador()
        {
            _notificacoes = [];
        }

        public bool TemNotificacoes() => _notificacoes.Count > 0;

        public void IncluirNotificacao(string mensagem)
        {
            _notificacoes.Add(mensagem);
        }

        public void LimparNotificacoes()
        {
            _notificacoes.Clear();
        }

        public void Dispose()
        {
            LimparNotificacoes();
            GC.SuppressFinalize(this);
        }
    }
}
