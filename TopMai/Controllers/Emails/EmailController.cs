using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Email;
using TopMai.Domain.Services.Emails.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Emails
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class EmailController : ControllerBase
    {
        #region Attributes
        private readonly IEmailService _emailService;
        #endregion

        #region Builder
        public EmailController(IEmailService addressService)
        {
            _emailService = addressService;
        }
        #endregion

        #region Services
        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmail(MailModelDto mail)
        {
            await _emailService.SendEmailAsync(mail);

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Error = string.Empty,
                Message = string.Empty,
                Result = string.Empty
            });

        }
        #endregion

    }
}
