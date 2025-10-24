using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.ErrosTransmissaoLote
{
    public class ErroTransmissaoLoteValidation : AbstractValidator<ErroTransmissaoLote>
    {
        public ErroTransmissaoLoteValidation()
        {
            ValidarInstituicao();
            ValidarVoluntario();
            ValidarMensagem();
        }

        private void ValidarInstituicao()
        {
            RuleFor(p => p.InstituicaoId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Favor preencher a instituição.");
        }

        private void ValidarVoluntario()
        {
            RuleFor(p => p.VoluntarioId)
                .NotEqual(Guid.Empty)
                    .When(p => p.VoluntarioId != null)
                        .WithMessage("Favor preencher o voluntário.");
        }
        
        private void ValidarMensagem()
        {
            RuleFor(p => p.Mensagem)
                .NotEmpty()
                    .WithMessage("Favor preencher a mensagem.");

            RuleFor(p => p.Mensagem)
                .MaximumLength(256)
                    .WithMessage("Mensagem deve ser preenchida com no máximo {MaxLength} caracteres.");
        }
    }
}
