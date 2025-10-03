using FluentValidation;
using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Core.Validations
{
    public abstract class InstituicaoEntityValidation<TEntity> : AbstractValidator<TEntity> where TEntity : InstituicaoEntity
    {
        public InstituicaoEntityValidation()
        {
            ValidarInstituicao();
        }

        private void ValidarInstituicao()
        {
            RuleFor(p => p.InstituicaoId)
                .NotEqual(Guid.Empty)
                    .WithMessage("Favor preencher a instituição.");
        }
    }
}
