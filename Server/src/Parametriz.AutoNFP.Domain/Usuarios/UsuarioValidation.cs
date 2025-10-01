using FluentValidation;
using Parametriz.AutoNFP.Domain.Core.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Usuarios
{
    public class UsuarioValidation : InstituicaoEntityValidation<Usuario>
    {
        public UsuarioValidation()
            : base()
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
