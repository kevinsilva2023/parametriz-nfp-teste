using FluentValidation;
using Parametriz.AutoNFP.Domain.Core.Validations;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Usuarios
{
    public class VoluntarioValidation : InstituicaoEntityValidation<Voluntario>
    {
        public VoluntarioValidation()
            : base()
        {
            ValidarNome();
            ValidarContato();
        }

        private void ValidarNome()
        {
            RuleFor(p => p.Nome)
                .MaximumLength(256)
                    .WithMessage("Nome deve ser preenchido com no máximo {MaxLength} caracteres.");
        }

        private void ValidarContato()
        {
            RuleFor(p => p.Contato)
                .NotEmpty()
                    .WithMessage("Favor preencher o telefone para contato.");

            RuleFor(p => p.Contato)
                .MaximumLength(11)
                    .WithMessage("Contato deve ser preenchido com no máximo {MaxLength} caracteres.");
        }
    }
}
