using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using TopMai.Domain.DTO.Email;
using TopMai.Domain.Services.Emails.Interfaces;

namespace TopMai.Domain.Services.Emails
{
    public class EmailService : IEmailService
    {
        #region Attributes

        private readonly IConfiguration _config;
        #endregion

        #region Builder
        public EmailService(IConfiguration configuration)
        {
            _config = configuration;
        }
        #endregion

        #region Methods
        public async Task SendEmailAsync(MailModelDto mail)
        {
            var _setting = _config.GetSection("EmailSettings");
            var client = new SmtpClient
            {
                Host = _setting.GetSection("Host").Value,
                Port = Convert.ToInt32(_setting.GetSection("Port").Value),
                Credentials = new NetworkCredential
                {
                    UserName = _setting.GetSection("User").Value,
                    Password = _setting.GetSection("Password").Value,
                },
                EnableSsl = true,
                Timeout = Convert.ToInt32(_setting.GetSection("Timeout").Value)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_setting.GetSection("EmailFrom").Value, _setting.GetSection("EmailName").Value),
                Subject = mail.Subject,
                Body = mail.Content,
                IsBodyHtml = mail.IsBodyHtml,
            };

            mail.To.ForEach(x =>
            {
                mailMessage.To.Add(new MailAddress(x.Addres, x.DisplayName));
            });

            if (mail.Files != null && mail.Files.Any())
            {
                mail.Files.ForEach(x =>
                {
                    mailMessage.Attachments.Add(new Attachment(x));
                });
            }

            await client.SendMailAsync(mailMessage);
        }


        #endregion
    }
}
