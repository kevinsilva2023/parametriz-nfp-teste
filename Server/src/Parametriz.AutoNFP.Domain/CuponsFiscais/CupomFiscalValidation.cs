using FluentValidation;
using Parametriz.AutoNFP.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.CuponsFiscais
{
    public class CupomFiscalValidation : AbstractValidator<CupomFiscal>
    {
        public CupomFiscalValidation()
        {
            ValidarInstituicaoId();
            ValidarCadastradoPorId();
            ValidarStatus();
            ValidarEnviadoEm();
            ValidarMensagemErro();
        }

        private void ValidarInstituicaoId()
        {
            RuleFor(p => p.InstituicaoId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Favor preencher a instituição.");
        }

        private void ValidarCadastradoPorId()
        {
            RuleFor(p => p.CadastradoPorId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Favor selecionar o usuário.");
        }

        private void ValidarStatus()
        {
            RuleFor(p => p.Status)
                .IsInEnum()
                    .WithMessage("Status do cupom fiscal inválido.");
        }

        private void ValidarEnviadoEm()
        {
            RuleFor(p => p.EnviadoEm)
                .NotNull()
                    .When(p => p.Status == CupomFiscalStatus.ENVIADO)
                        .WithMessage("Favor preencher a data de envio.");
        }

        private void ValidarMensagemErro()
        {
            RuleFor(p => p.MensagemErro)
                .NotEmpty()
                    .When(p => p.Status == CupomFiscalStatus.ERRO)
                        .WithMessage("Favor preencher a mensagem do erro.");
        }
    }
}
