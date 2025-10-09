using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
using Parametriz.AutoNFP.Domain.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Repository
{
    public class UsuarioRepository : InstituicaoEntityRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AutoNfpDbContext context) 
            : base(context)
        {
        }

        public override async Task<bool> EhUnico(Usuario usuario)
        {
            return  !await _context.Usuarios
                .AnyAsync(u => u.InstituicaoId == usuario.InstituicaoId &&
                               u.Nome == usuario.Nome &&
                               u.Id != usuario.Id);
        }
    }
}
