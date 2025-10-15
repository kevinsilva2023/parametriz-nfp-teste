using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.CuponsFiscais
{
    public class CupomFiscalPaginacao
    {
        public IReadOnlyCollection<CupomFiscal> CuponsFiscais { get; private set; }

        public int Pagina { get; private set; }
        public int RegistrosPorPagina { get; private set; }

        public int Processando { get; private set; }
        public int Sucesso { get; private set; }
        public int Erro { get; private set; }
        public int Total { get; private set; }

        public CupomFiscalPaginacao(IEnumerable<CupomFiscal> cuponsFiscais, int pagina, int registrosPorPagina,
            int processando, int sucesso, int erro, int total)
        {
            CuponsFiscais = cuponsFiscais.ToList();
            Pagina = pagina;
            RegistrosPorPagina = registrosPorPagina;
            Processando = processando;
            Sucesso = sucesso;
            Erro = erro;
            Total = total;
        }
    }
}
