using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.CuponsFiscais
{
    public class CupomFiscal : InstituicaoEntity
    {
        public ChaveDeAcesso ChaveDeAcesso { get; private set; }
        public Guid CadastradoPorId { get; private set; }
        public CupomFiscalStatus Status { get; private set; }
        public DateTime? EnviadoEm { get; private set; }
        public string MensagemErro { get; private set; }
        
        public Usuario CadastradoPor { get; private set; }

        public CupomFiscal(Guid id, Guid instituicaoId, string chave, Guid cadastradoPorId)
            : base(id, instituicaoId)
        {
            ChaveDeAcesso = new ChaveDeAcesso(chave);
            CadastradoPorId = cadastradoPorId;
            AlterarStatus(CupomFiscalStatus.PROCESSANDO);
        }

        protected CupomFiscal() { }

        public void AlterarStatus(CupomFiscalStatus status, string mensagemErro = "")
        {
            Status = status;

            EnviadoEm = status switch
            {
                CupomFiscalStatus.SUCESSO => DateTime.Now,
                _ => null
            };

            MensagemErro = status switch 
            {
                CupomFiscalStatus.ERRO => mensagemErro.Trim(),
                _ => string.Empty
            };
        }
    }
}
