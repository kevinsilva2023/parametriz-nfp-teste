
using System.Net.Mail;

namespace Parametriz.AutoNFP.Api.Application.Email.Services
{
    public interface IEmailService
    {
        Task Enviar(string emailPara, string assunto, string corpo, IEnumerable<Attachment> anexos = null, 
            IEnumerable<string> copiasOcultasPara = null, string responderPara = "");

        Task Enviar(IEnumerable<string> emailsPara, string assunto, string corpo, IEnumerable<Attachment> anexos = null, 
            IEnumerable<string> copiasOcultasPara = null, string responderPara = "");
    }
}
