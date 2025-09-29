using Parametriz.AutoNFP.Api.ViewModels.Core;
using Parametriz.AutoNFP.Domain.Usuarios;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Usuarios
{
    public class UsuarioViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(256, ErrorMessage = "Nome deve ser preenchido com no máximo {1} caracteres.")]
        public string Nome { get; set; }

        public EmailViewModel Email { get; set; }



        public Usuario ViewModelToDomain() => new(Id, Nome, Email.ViewModelToDomain());
    }
}
