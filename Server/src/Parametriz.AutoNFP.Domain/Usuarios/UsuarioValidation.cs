using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Usuarios
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            ValidarNome();
        }

        private void ValidarNome()
        {
            RuleFor(p => p.Nome)
                .MaximumLength(256)
                    .WithMessage("Nome deve ser preenchido com no máximo {MaxLength} caracteres.");
        }
    }
}
