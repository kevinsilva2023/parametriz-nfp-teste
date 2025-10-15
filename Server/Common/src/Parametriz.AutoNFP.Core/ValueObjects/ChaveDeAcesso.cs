using FluentValidation;
using Parametriz.AutoNFP.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Core.ValueObjects
{
    public class ChaveDeAcesso
    {
        public string Chave { get; private set; }

        public bool ExisteChave { get; private set; }

        public int UfCodigo => ExisteChave ? Convert.ToInt32(Chave.Substring(0, 2)) : 0;
        public DateTime? MesEmissao { get; private set; }
        public DateTime? LimiteEnvio => MesEmissao != null ?
            new DateTime(MesEmissao.Value.AddMonths(1).Year, MesEmissao.Value.AddMonths(1).Month, 20).Date : null;
        public CnpjCpf Cnpj { get; private set; }
        public int Modelo => ExisteChave ? Convert.ToInt32(Chave.Substring(20, 2)) : 0;
        public int Serie => ExisteChave ? Convert.ToInt32(Chave.Substring(22, 3)) : 0;
        public int Numero => ExisteChave ? Convert.ToInt32(Chave.Substring(25, 9)) : 0;
        public int FormaEmissao => ExisteChave ? Convert.ToInt32(Chave.Substring(34, 1)) : 0;
        public int CodigoNumerico => ExisteChave ? Convert.ToInt32(Chave.Substring(35, 8)) : 0;
        public int DigitoVerificador => ExisteChave ? Convert.ToInt32(Chave.Substring(43, 1)) : 0;

        public ChaveDeAcesso(string chave)
        {
            Chave = new string(chave.Where(c => char.IsDigit(c)).ToArray());

            ExisteChave = !string.IsNullOrEmpty(Chave) && Chave.Length == 44;

            if (ExisteChave)
            {
                MesEmissao = new DateTime(2000 + Convert.ToInt32(Chave.Substring(2, 2)), Convert.ToInt32(Chave.Substring(4, 2)), 1).Date;
                Cnpj = new CnpjCpf(TipoPessoa.Juridica, Chave.Substring(6, 14));
            }
        }

        protected ChaveDeAcesso() { }

        public static bool ValidarChave(string numero)
        {
            var numeros = new int[44];
            var numerosVerificacao = new int[]
            {
                2, 3, 4, 5, 6, 7, 8, 9,
                2, 3, 4, 5, 6, 7, 8, 9,
                2, 3, 4, 5, 6, 7, 8, 9,
                2, 3, 4, 5, 6, 7, 8, 9,
                2, 3, 4, 5, 6, 7, 8, 9,
                2, 3, 4
            };
            var resultadoMultiplicacao = 0;

            for (var i = 0; i < 44; i++)
                if (!int.TryParse(numero[i].ToString(), out numeros[i]))
                    return false;

            for (var i = 0; i < 43; i++)
                resultadoMultiplicacao += numeros[i] * numerosVerificacao[42 - i];

            var resultadoVerificacao = resultadoMultiplicacao % 11;

            if (resultadoVerificacao < 2)
            {
                if (numeros[43] != 0)
                    return false;
            }
            else
            {
                if (numeros[43] != 11 - resultadoVerificacao)
                    return false;
            }

            return true;
        }
    }

    public class ChaveDeAcessoValidation : AbstractValidator<ChaveDeAcesso>
    {
        public ChaveDeAcessoValidation()
        {
            ValidarChave();
            ValidarUfCofigo();
            ValidarLmiteEnvio();
            ValidarModelo();
        }

        private void ValidarChave()
        {
            RuleFor(p => p.Chave)
                .NotEmpty()
                    .WithMessage("Favor preencher a chave");

            RuleFor(p => p.Chave)
                .Length(44)
                    .WithMessage("Chave deve ser preenchida com {Length} digitos.");

            RuleFor(p => p.Chave)
                .Must(chave => ChaveDeAcesso.ValidarChave(chave))
                    .When(p => p.ExisteChave)
                        .WithMessage("Chave de acesso inválida.");
        }

        private void ValidarUfCofigo()
        {
            RuleFor(p => p.UfCodigo)
                .Equal(35)
                    .When(p => p.ExisteChave)
                        .WithMessage("UF da chave diferente de SP.");
        }

        private void ValidarLmiteEnvio()
        {
            RuleFor(p => p.LimiteEnvio)
                .Must(p => p.Value.Date >= DateTime.Now.Date)
                    .When(p => p.ExisteChave)
                        .WithMessage("Ultrapassou a data limite de envio.");
        }

        private void ValidarModelo()
        {
            RuleFor(p => p.Modelo)
                .Must(m => new List<int> { 59, 65 }.Contains(m))
                    .When(p => p.ExisteChave)
                        .WithMessage("Modelo do documento fiscal inválido.");
        }
    }
}
