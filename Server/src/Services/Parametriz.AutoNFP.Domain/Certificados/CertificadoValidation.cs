using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Certificados
{
    public class CertificadoValidation : AbstractValidator<Certificado>
    {
        public CertificadoValidation()
        {
            ValidarNome();
            ValidarRequerente();
            ValidarValidade();
            ValidarEmissor();
            ValidarUpload();
            ValidarSenha();
        }

        private void ValidarNome()
        {
            RuleFor(p => p.Nome)
                .NotEmpty()
                    .WithMessage("Favor preencher o nome.");

            RuleFor(p => p.Nome)
                .MaximumLength(256)
                    .WithMessage("Nome deve ser preenchido com no máximo {MaxLength} caracteres.");
        }

        private void ValidarRequerente()
        {
            RuleFor(p => p.Requerente)
               .NotEmpty()
                   .WithMessage("Favor preencher o requerente.");

            RuleFor(p => p.Nome)
                .MaximumLength(256)
                    .WithMessage("Requerente deve ser preenchido com no máximo {MaxLength} caracteres.");
        }

        private void ValidarValidade()
        {
            RuleFor(p => p.ValidoAte)
                .GreaterThan(p => p.ValidoAPartirDe)
                    .WithMessage("Valido até deve ser maior do que valido a partir de.");
        }

        private void ValidarEmissor()
        {
            RuleFor(p => p.Emissor)
                .NotEmpty()
                    .WithMessage("Favor preencher o emissor.");

            RuleFor(p => p.Emissor)
                .MaximumLength(256)
                    .WithMessage("Emissor deve ser preenchido com no máximo {MaxLength} caracteres.");
        }

        private void ValidarUpload()
        {
            RuleFor(p => p.Upload)
                .NotEmpty()
                    .WithMessage("Favor preencher o upload.");

            RuleFor(p => p.Upload.LongLength)
                .LessThanOrEqualTo(25600)
                    .WithMessage("Arquivo do certificado excede o tamanho limite de 25Kb.");
        }

        private void ValidarSenha()
        {
            RuleFor(p => p.Senha)
                .NotEmpty()
                    .WithMessage("Favor preencher a senha.");
        }
    }
}
