
using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.Api.Configs;
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using System.Net;
using System.Net.Mail;

namespace Parametriz.AutoNFP.Api.Application.Email.Services
{
    public class EmailService : BaseService, IEmailService
    {
        private readonly SmtpConfig _smtpConfig;

        public EmailService(IAspNetUser user, 
                            IUnitOfWork uow, 
                            Notificador notificador,
                            IOptions<SmtpConfig> options) 
            : base(user, uow, notificador)
        {
            _smtpConfig = options.Value;
        }

        private MailMessage CriarMailMessageSemDestinatario(string assunto, string corpo, IEnumerable<Attachment> anexos = null,
            IEnumerable<string> copiasOcultasPara = null, string responderPara = "")
        {
            var message = new MailMessage();

            message.From = new MailAddress(_smtpConfig.Address, _smtpConfig.DisplayName);

            if (!string.IsNullOrWhiteSpace(responderPara))
                message.ReplyToList.Add(responderPara);

            if (copiasOcultasPara != null)
                foreach (var cco in copiasOcultasPara)
                    message.Bcc.Add(new MailAddress(cco));

            message.Subject = assunto;

            if (anexos != null)
                foreach (var anexo in anexos)
                    message.Attachments.Add(anexo);

            message.IsBodyHtml = true;

            message.Body = corpo;

            return message;
        }

        private SmtpClient CriarSmtpClient()
        {
            var smtp = new SmtpClient(_smtpConfig.Host, _smtpConfig.Port);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

            return smtp;
        }

        private bool EmailAptoParaEnviar(MailMessage message)
        {
            if (string.IsNullOrEmpty(message.From.Address))
                return NotificarErro("Favor preencher o remetente.");

            if (!message.To.Any() && !message.Bcc.Any())
                return NotificarErro("E-mail sem destinatários.");

            return CommandEhValido();
        }

        public virtual async Task Enviar(IEnumerable<string> emailsPara, string assunto, string corpo,
            IEnumerable<Attachment> anexos = null, IEnumerable<string> copiasOcultasPara = null,
            string responderPara = "")
        {
            try
            {
                using var message = CriarMailMessageSemDestinatario(assunto, corpo, anexos, copiasOcultasPara, responderPara);

                foreach (string email in emailsPara)
                    message.To.Add(new MailAddress(email));

                if (!EmailAptoParaEnviar(message))
                    return;

                using var smtp = CriarSmtpClient();
                
                await smtp.SendMailAsync(message);
            }
            catch
            {
                NotificarErro($"Ocorreu um erro ao enviar o e-mail.");
            }
        }

        public virtual async Task Enviar(string emailPara, string assunto, string corpo, 
            IEnumerable<Attachment> anexos = null, IEnumerable<string> copiasOcultasPara = null, string responderPara = "")
        {
            try
            {
                using var message = CriarMailMessageSemDestinatario(assunto, corpo, anexos, copiasOcultasPara, responderPara);

                message.To.Add(new MailAddress(emailPara));

                if (!EmailAptoParaEnviar(message))
                    return;
                
                using var smtp = CriarSmtpClient();

                await smtp.SendMailAsync(message);
            }
            catch
            {
                NotificarErro($"Ocorreu um erro ao enviar o e-mail.");
            }
        }
    }
}
