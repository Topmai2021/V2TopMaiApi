using Common.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.CodeValidation;
using TopMai.Domain.Services.Users.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Enums.Enums;

namespace TopMai.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class CodeValidationController : ControllerBase
    {
        #region Attributes
        private readonly ICodeValidationServices _codeValidation;
        #endregion

        #region Builder
        public CodeValidationController(ICodeValidationServices codeValidationServices)
        {
            _codeValidation = codeValidationServices;
        }
        #endregion

        #region Services
        [HttpGet]
        [Route("GenerateCodeEmailByIdUser")]
        public async Task<IActionResult> GenerateCodeEmail(Guid idUser)
        {
            IActionResult action;
            bool result = await _codeValidation.GenerateCodeEmail(idUser);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? "Mensaje Enviado al correo suministrado" : "Tuvimos problemas para generar el código, por favor vuelva a intentarlo"
            };
            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpGet]
        [Route("GenerateCodeEmailByEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateCodeEmail(string email)
        {
            IActionResult action;
            bool result = await _codeValidation.GenerateCodeEmail(email);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? "Mensaje Enviado al correo suministrado" : "Tuvimos problemas para generar el código, por favor vuelva a intentarlo"
            };
            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpPost]
        [Route("ValidateCodeEmail")]
        [ServiceFilter(typeof(CustomValidationFilterAttribute))]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateCodeEmail(ValidationCodeDto validationCodeDto)
        {
            IActionResult action;
            var dto = new ValidationCode()
            {
                UserId = validationCodeDto.UserId,
                Code = validationCodeDto.Code,
                Type = TypeCodeValidation.Email
            };

            var  (isValid, userEmail) = await _codeValidation.ValidateCode(dto);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = isValid,
                Result = isValid,
                Message = isValid ? "Código Válidado correctamente." : "Hubo un problema, por favor intentarlo de nuevo"
            };
            if (isValid)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpGet]
        [Route("GenerateCodePhoneByIdUser")]
        public async Task<IActionResult> GenerateCodePhoneByIdUser(Guid idUser)
        {
            var result = await _codeValidation.GenerateCodePhone(idUser);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Result = result,
                Message = "Código generado correctamente"
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("GenerateCodePhoneByPhone")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateCodePhoneByPhone(string phone)
        {
            var result = await _codeValidation.GenerateCodePhone(phone);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Result = result,
                Message = "Código generado correctamente"
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("ValidateCodePhone")]
        [ServiceFilter(typeof(CustomValidationFilterAttribute))]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateCodePhone(ValidationCodeDto validate)
        {
            IActionResult action;

            var validationCode = new ValidationCode()
            {
                UserId = validate.UserId,
                Code = validate.Code,
                Type = TypeCodeValidation.Phone
            };

            var (isValid, userPhone) = await _codeValidation.ValidateCode(validationCode);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = isValid,
                Result = isValid,
                Message = isValid ? "Código Válidado correctamente." : "Hubo un problema, por favor intentarlo de nuevo"
            };
            if (isValid)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }
        #endregion
    }
}
