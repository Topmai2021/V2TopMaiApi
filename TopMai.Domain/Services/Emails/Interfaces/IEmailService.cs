using TopMai.Domain.DTO.Email;

namespace TopMai.Domain.Services.Emails.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailModelDto mail);
    }
}
