using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Core.ValueObjects
{
    public class Endereco
    {
        public string TipoLogradouro { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string CEP { get; private set; }
        public string Municipio { get; private set; }
        public string UF { get; private set; }

        protected Endereco() { }

        public Endereco(string tipoLogradouro, string logradouro, string numero, string complemento,
            string bairro, string cep, string municipio, string uf)
        {
            TipoLogradouro = tipoLogradouro;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            CEP = cep.Replace(".", "").Replace("-", "").PadLeft(8, '0');
            Municipio = municipio;
            UF = uf.ToUpper();
        }
    }

    public class EnderecoValidation : AbstractValidator<Endereco>
    {
        public EnderecoValidation()
        {
            ValidarTipoLogradouro();
            ValidarLogradouro();
            ValidarNumero();
            ValidarComplemento();
            ValidarBairro();
            ValidarCEP();
            ValidarMunicipio();
            ValidarUF();
        }

        private void ValidarTipoLogradouro()
        {
            RuleFor(p => p.TipoLogradouro)
                .NotEmpty().WithMessage("Favor preencher o tipo de logradouro.")
                .Length(2, 15).WithMessage("O tipo de logradouro deve ser preenchido com no mínimo 2 e no máximo 15 caracteres.");
        }

        private void ValidarLogradouro()
        {
            RuleFor(p => p.Logradouro)
                .NotEmpty().WithMessage("Favor preencher o logradouro.")
                .Length(2, 100).WithMessage("O logradouro deve ser preenchido com no mínimo 2 e no máximo 100 caracteres.");
        }

        private void ValidarNumero()
        {
            RuleFor(p => p.Numero)
                .NotEmpty().WithMessage("Favor preencher o número.")
                .MaximumLength(10).WithMessage("O número deve ser preenchido com no máximo 10 caracteres.");
        }

        private void ValidarComplemento()
        {
            RuleFor(p => p.Complemento)
                .MaximumLength(50).When(p => !string.IsNullOrEmpty(p.Complemento))
                              .WithMessage("O complemento deve ser preenchido com no máximo 50 caracteres.");
        }

        private void ValidarBairro()
        {
            RuleFor(p => p.Bairro)
                .NotEmpty().WithMessage("Favor preencher o bairro.")
                .Length(2, 100).WithMessage("O bairro deve ser preenchido com no mínimo 2 e no máximo 100 caracteres.");
        }

        private void ValidarCEP()
        {
            RuleFor(p => p.CEP)
                .NotEqual("00000000").WithMessage("Favor preencher o CEP.")
                .Length(8).WithMessage("O CEP deve ser preenchido com 8 digitos numércios.")
                .Matches("^[0-9]{8}$").WithMessage("O CEP deve ser preenchido somente com digitos númericos.");
        }

        private void ValidarMunicipio()
        {
            RuleFor(p => p.Municipio)
                .NotEmpty().WithMessage("Favor preencher o município.")
                .Length(2, 50).WithMessage("O município deve ser preenchido com no mínimo 2 e no máximo 50 caracteres.");
        }

        private void ValidarUF()
        {
            RuleFor(p => p.UF)
                .NotEmpty().WithMessage("Favor preencher a UF.")
                .Length(2).WithMessage("A UF deve ser preenchida com 2 caracteres.");

        }
    }
}
