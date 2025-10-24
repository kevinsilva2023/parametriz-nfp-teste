using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.ErrosTransmissaoLote
{
    public class ErroTransmissaoLote : InstituicaoEntity
    {
        public Guid? VoluntarioId { get; private set; }
        public DateTime Data { get; private set; }
        public string Mensagem { get; private set; }

        public Voluntario Voluntario { get; private set; }

        protected ErroTransmissaoLote(Guid id, Guid instituicaoId)
            : base(id, instituicaoId)
        {
            Data = DateTime.Now;
        }

        public ErroTransmissaoLote(Guid id, Guid instituicaoId, string mensagem)
            : this(id, instituicaoId)
        {
            Mensagem = mensagem;
        }

        public ErroTransmissaoLote(Guid id, Guid instituicaoId, Guid voluntarioId, string mensagem)
            : this(id, instituicaoId, mensagem)
        {
            VoluntarioId = voluntarioId;
        }

        protected ErroTransmissaoLote() { }
    }
}
