using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Instituicoes
{
    public class InstituicaoValidation : AbstractValidator<Instituicao>
    {
        public InstituicaoValidation()
        {
            ValidarRazaoSocial();
            ValidarEntidadeNomeNFP();
            
        }

        private void ValidarRazaoSocial()
        {
            RuleFor(p => p.RazaoSocial)
                .NotEmpty()
                    .WithMessage("Favor preencher a razão social.");

            RuleFor(p => p.RazaoSocial)
                .MaximumLength(256)
                    .WithMessage("Razão social deve ser preenchida com no máximo {MaxLength} caracteres.");
        }


        private void ValidarEntidadeNomeNFP()
        {
            RuleFor(p => p.EntidadeNomeNFP)
                .NotEmpty()
                    .WithMessage("Favor preencher o nome da entidade na nota fiscal paulista.");

            RuleFor(p => p.EntidadeNomeNFP)
                .MaximumLength(256)
                    .WithMessage("Nome da entidade na nota fiscal paulista deve ser preenchido com no máximo {MaxLength} caracteres.");
        }
    }
}
