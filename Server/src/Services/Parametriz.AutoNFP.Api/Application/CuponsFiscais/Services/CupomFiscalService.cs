using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.CuponsFiscais;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.CuponsFiscais;

namespace Parametriz.AutoNFP.Api.Application.CuponsFiscais.Services
{
    public class CupomFiscalService : BaseService, ICupomFiscalService
    {
        private readonly ICupomFiscalRepository _cupomFiscalRepository;

        public CupomFiscalService(IAspNetUser user,
                                  IUnitOfWork uow,
                                  Notificador notificador,
                                  ICupomFiscalRepository cupomFiscalRepository)
            : base(user, uow, notificador)
        {
            _cupomFiscalRepository = cupomFiscalRepository;
        }

        private async Task ValidarCupomFiscal(CupomFiscal cupomFiscal)
        {
            await ValidarEntidade(new CupomFiscalValidation(), cupomFiscal);
            await ValidarEntidade(new ChaveDeAcessoValidation(), cupomFiscal.ChaveDeAcesso);
            await ValidarEntidade(new CnpjCpfObrigatorioValidation(), cupomFiscal.ChaveDeAcesso.Cnpj);
        }

        private async Task CupomFiscalEhUnico(CupomFiscal cupomFiscal)
        {
            if (!await _cupomFiscalRepository.EhUnico(cupomFiscal))
                NotificarErro("Cupom Fiscal já cadastrado.");
        }

        private async Task<bool> CupomFiscalAptoParaCadastrar(CupomFiscal cupomFiscal)
        {
            await ValidarCupomFiscal(cupomFiscal);
            await CupomFiscalEhUnico(cupomFiscal);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(CadastrarCupomFiscalViewModel cadastrarCupomFiscalViewModel)
        {
            var chave = ExtrairChave(cadastrarCupomFiscalViewModel.QrCode.Trim());

            if (string.IsNullOrEmpty(chave))
                return NotificarErro("Não foi possível verificar a chave de acesso.");

            var cupomFiscal = new CupomFiscal(Guid.NewGuid(), InstituicaoId, chave, UsuarioId);

            if (!await CupomFiscalAptoParaCadastrar(cupomFiscal))
                return false;

            await _cupomFiscalRepository.Cadastrar(cupomFiscal);

            await Commit();

            return CommandEhValido();
        }

        private string ExtrairChave(string qrCode)
        {
            return qrCode.Substring(0, 3).ToUpper() switch
            {
                "HTT" => ExtrairChaveNFCe(qrCode),
                "CFE" => ExtrairChaveCFe(qrCode),
                _ => ExtrairSAT(qrCode)
            };
        }

        private string ExtrairChaveNFCe(string qrCode)
        {
            var blocos = qrCode.Split("|");

            if (blocos.Count() <= 1)
                return string.Empty;

            if (blocos[2] != "1")
                return string.Empty;

            var primeiroBloco = blocos[0].ToUpper().Split("P=");

            if (primeiroBloco.Count() < 2)
                return string.Empty;

            if (primeiroBloco[1].Length != 44)
                return string.Empty;

            return primeiroBloco[1];
        }

        private string ExtrairChaveCFe(string qrCode)
        {
            var blocos = qrCode.Split("|");

            if (blocos.Count() <= 1)
                return string.Empty;

            var chave = blocos[0]; //.ToUpper().Replace("CFE", "");

            if (chave.Length != 44)
                return string.Empty;

            return chave;
        }

        private string ExtrairSAT(string qrCode)
        {
            var blocos = qrCode.Split("|");

            if (blocos.Count() <= 1)
                return string.Empty;

            if (blocos[0].Length != 44)
                return string.Empty;

            return blocos[0];
        }
    }
}
