using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.FileSistem
{
    public class FileSystemService : BaseService, IFileSystemService
    {
        public FileSystemService(IUnitOfWork uow, 
                                 Notificador notificador) 
            : base(uow, notificador)
        {
        }

        public bool ExecutarProcesso(Voluntario voluntario, string senha)
        {
            throw new NotImplementedException();
        }
    }
}
