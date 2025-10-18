using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.FileSistem
{
    public interface IFileSystemService
    {
        bool ExecutarProcesso(Voluntario voluntario, string senha, int port);
    }
}
