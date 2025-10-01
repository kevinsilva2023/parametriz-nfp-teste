using FluentValidation;
using Parametriz.AutoNFP.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Core.ValueObjects
{
    public class CnpjCpf
    {
        public TipoPessoa TipoPessoa { get; private set; }
        public string NumeroInscricao { get; private set; }

        public CnpjCpf(TipoPessoa tipoPessoa, string numeroInscricao)
        {
            TipoPessoa = tipoPessoa;
            switch (tipoPessoa)
            {
                case TipoPessoa.Fisica:
                    NumeroInscricao = !string.IsNullOrEmpty(numeroInscricao) ? numeroInscricao.Replace(".", "").Replace("-", "").PadLeft(11, '0') : null;
                    break;
                case TipoPessoa.Juridica:
                    NumeroInscricao = !string.IsNullOrEmpty(numeroInscricao) ? numeroInscricao.Replace(".", "").Replace("/", "").Replace("-", "").PadLeft(14, '0') : null;
                    break;
                default:
                    NumeroInscricao = string.Empty;
                    break;
            }
        }

        protected CnpjCpf() { }

        public static bool ValidarCpf(string numero)
        {
            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
                if (numero[i] != numero[0])
                    igual = false;

            if (igual || numero == "12345678909")
                return false;

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                if (!int.TryParse(numero[i].ToString(), out numeros[i]))
                    return false;

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else
            {
                if (numeros[9] != 11 - resultado)
                    return false;
            }

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0) return false;
            }
            else
            {
                if (numeros[10] != 11 - resultado) return false;
            }
            return true;
        }

        public static bool ValidarCnpj(string numero)
        {
            var igual = true;
            for (var i = 1; i < 14; i++)
                if (numero[i] != numero[0])
                    igual = false;

            if (igual)
                return false;

            var numeros = new int[14];
            var numerosVerificacao = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var resultadoMultiplicacao = 0;

            for (var i = 0; i < 14; i++)
                if (!int.TryParse(numero[i].ToString(), out numeros[i]))
                    return false;

            for (var i = 0; i < 12; i++)
                resultadoMultiplicacao += numeros[i] * numerosVerificacao[i + 1];

            var resultadoVerificacao = resultadoMultiplicacao % 11;
            if (resultadoVerificacao < 2)
            {
                if (numeros[12] != 0)
                    return false;
            }
            else
            {
                if (numeros[12] != 11 - resultadoVerificacao)
                    return false;
            }

            resultadoMultiplicacao = 0;

            for (var i = 0; i < 13; i++)
                resultadoMultiplicacao += numeros[i] * numerosVerificacao[i];

            resultadoVerificacao = resultadoMultiplicacao % 11;
            if (resultadoVerificacao < 2)
            {
                if (numeros[13] != 0)
                    return false;
            }
            else
            {
                if (numeros[13] != 11 - resultadoVerificacao)
                    return false;
            }

            return true;
        }
    }

    public class CnpjCpfObrigatorioValidation : AbstractValidator<CnpjCpf>
    {
        public CnpjCpfObrigatorioValidation()
        {
            ValidarTipoPessoa();
            ValidarNumero();
        }

        private void ValidarTipoPessoa()
        {
            RuleFor(p => p.TipoPessoa)
                .NotEmpty().WithMessage("Favor preencher o tipo de pessoa.");
        }

        private void ValidarNumero()
        {
            RuleFor(p => p.NumeroInscricao)
                .NotEmpty().WithMessage("Favor preencher o número de inscrição no CPF ou CNPJ dependendo do tipo de pessoa.");

            RuleFor(p => p.NumeroInscricao)
                .Length(11).When(p => p.TipoPessoa == TipoPessoa.Fisica).WithMessage("CPF deve ser preenchido com 11 digitos numéricos.")
                .Matches("^[0-9]{11}$").When(p => p.TipoPessoa == TipoPessoa.Fisica).WithMessage("CPF deve ser preenchido com 11 digitos numéricos.")
                .Must(numero => CnpjCpf.ValidarCpf(numero)).When(p => p.TipoPessoa == TipoPessoa.Fisica).WithMessage("Número de inscrição no CPF inválido.");

            RuleFor(p => p.NumeroInscricao)
                .Length(14).When(p => p.TipoPessoa == TipoPessoa.Juridica).WithMessage("CNPJ deve ser preenchido com 14 digitos numéricos.")
                .Matches("^[0-9]{14}$").When(p => p.TipoPessoa == TipoPessoa.Juridica).WithMessage("CNPJ deve ser preenchido com 14 digitos numéricos.")
                .Must(numero => CnpjCpf.ValidarCnpj(numero)).When(p => p.TipoPessoa == TipoPessoa.Juridica).WithMessage("Número de inscrição no CNPJ inválido");
        }
    }

    public class CnpjCpfOpicionalValidation : AbstractValidator<CnpjCpf>
    {
        public CnpjCpfOpicionalValidation()
        {
            ValidarTipoPessoa();
            ValidarNumero();
        }

        private void ValidarTipoPessoa()
        {
            RuleFor(p => p.TipoPessoa)
                .NotEmpty().When(p => p.NumeroInscricao != string.Empty).WithMessage("Favor preencher o tipo de pessoa.");
        }

        private void ValidarNumero()
        {
            RuleFor(p => p.NumeroInscricao)
                .Length(11).When(p => p.TipoPessoa == TipoPessoa.Fisica && !string.IsNullOrEmpty(p.NumeroInscricao)).WithMessage("CPF deve ser preenchido com 11 digitos numéricos.")
                .Matches("^[0-9]{11}$").When(p => p.TipoPessoa == TipoPessoa.Fisica && !string.IsNullOrEmpty(p.NumeroInscricao)).WithMessage("CPF deve ser preenchido com 11 digitos numéricos.")
                .Must(numero => CnpjCpf.ValidarCpf(numero)).When(p => p.TipoPessoa == TipoPessoa.Fisica && !string.IsNullOrEmpty(p.NumeroInscricao)).WithMessage("Número de inscrição no CPF inválido.");

            RuleFor(p => p.NumeroInscricao)
                .Length(14).When(p => p.TipoPessoa == TipoPessoa.Juridica && !string.IsNullOrEmpty(p.NumeroInscricao)).WithMessage("CNPJ deve ser preenchido com 14 digitos numéricos.")
                .Matches("^[0-9]{14}$").When(p => p.TipoPessoa == TipoPessoa.Juridica && !string.IsNullOrEmpty(p.NumeroInscricao)).WithMessage("CNPJ deve ser preenchido com 14 digitos numéricos.")
                .Must(numero => CnpjCpf.ValidarCnpj(numero)).When(p => p.TipoPessoa == TipoPessoa.Juridica && !string.IsNullOrEmpty(p.NumeroInscricao)).WithMessage("Número de inscrição no CNPJ inválido");
        }
    }
}
