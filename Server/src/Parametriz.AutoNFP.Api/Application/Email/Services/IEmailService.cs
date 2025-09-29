
namespace Parametriz.AutoNFP.Api.Application.Email.Services
{
    public interface IEmailService
    {
        Task Enviar(string email, string assunto, string corpo);
    }
}
