using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Core.ValueObjects
{
    public class Email
    {
        public string Conta { get; private set; }

        public Email(string conta)
        {
            Conta = conta.Trim().ToLower();
        }

        protected Email() { }
    }

    public class EmailValidation : AbstractValidator<Email>
    {
        public EmailValidation()
        {
            ValidarEndereco();
        }

        private void ValidarEndereco()
        {
            RuleFor(p => p.Conta)
                .NotEmpty()
                    .WithMessage("Favor preencher o e-mail.");

            RuleFor(p => p.Conta)
                .Matches(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")
                    .WithMessage("Favor preencher um e-mail valido.");

            RuleFor(p => p.Conta)
                .MaximumLength(256)
                    .WithMessage("E-mail deve ter no máximo {MaxLength} caracteres.");
        }
    }
}
