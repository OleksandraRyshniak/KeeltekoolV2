using Keeltekool_2.Core.DTO;
using Keeltekool_2.Core.ServiceInterface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Keeltekool_2.ApplicationServices.Services
{
    public class EmailingServices : IEmailingServices
    {
        private readonly IConfiguration _config;

        public EmailingServices(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailConfiguration:From"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;

            var builder = new BodyBuilder { HtmlBody = request.Body };

            foreach (var file in request.Attachment)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                builder.Attachments.Add(file.FileName, ms.ToArray(), ContentType.Parse(file.ContentType));
            }

            email.Body = builder.ToMessageBody();
            Send(email);
        }

        public void SendEmailToken(EmailTokenDTO request, string token)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailConfiguration:From"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = request.Body };
            Send(email);
        }

        private void Send(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(
                _config["EmailConfiguration:SmtpServer"],
                int.Parse(_config["EmailConfiguration:Port"]!),
                SecureSocketOptions.StartTls);
            smtp.Authenticate(
                _config["EmailConfiguration:Username"],
                _config["EmailConfiguration:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}