using Common.Utils.Resources;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.DTO.User;
using TopMai.Domain.Services.Users.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;
using static Humanizer.In;

namespace TopMai.Controllers.User
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class UserController : ControllerBase
    {
        #region Attributes
        private readonly IUserService _userService;
        #endregion

        #region Builder
        public UserController(IUserService userService)
        {
            _userService = userService;

        }

        #endregion

        #region Services

        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> GetAll(int page = 1, int limit = 100000000)
        {
            var (users, totalCount) = await _userService.GetAll(page, limit);
            var response = new
            {

                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = limit,
                Data = users
            };
            return Ok(response);
        }


        [HttpPost("GetAdminDashbaordData")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult GetAdminDashbaordData()
        {
            Dashboard dashboardData = _userService.GetAdminDashboardData();

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Result = dashboardData
            });
        }


        [HttpGet("get")]
        public ActionResult Get(Guid idUser)
        {
            UserDto userDto = _userService.Get(idUser);

            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = userDto
            });
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(AddUser_Dto? user)
        {
            TokenDto token = await _userService.Post(user);

            var response = new ResponseDto
            {
                IsSuccess = token != null,
                Message = token != null ? "Usuario Registrado" : "No logro registarse con exito",
                Error = string.Empty,
                Result = token
            };
            return Ok(response);
        }

        [HttpPost("socialRegister")]
        [AllowAnonymous]
        public async Task<IActionResult> SocialRegister([FromBody] SocialRegister socialRegister)
        {
            TokenDto token = await _userService.SocialRegister(socialRegister);

            if (token is null)
            {
                return BadRequest(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "No logro registarse con exito",
                    Error = string.Empty,
                    Result = null
                });
            }

            return Ok(new ResponseDto
            {
                IsSuccess = true,
                Message = "Usuario Registrado",
                Error = string.Empty,
                Result = token
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            TokenDto result = _userService.Login(loginRequest.accessData, loginRequest.password);

            return Ok(result);
        }

        [HttpPost("socialLogin")]
        [AllowAnonymous]
        public ActionResult SocialLogin([FromBody] SocialLoginRequest socialLoginRequest)
        {
            TokenDto result = _userService.SocialLogin(socialLoginRequest.SocialUserId, socialLoginRequest.Email);

            return Ok(result);
        }

        [HttpPost("changeForgottenPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeForgottenPassword(ChangeForgottenPasswordDto change)
        {
            await _userService.ChangeForgottenPassword(change);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Result = true,
                Message = GeneralMessages.ChangedPassword,
                Error = GeneralMessages.ChangedPassword,
            };
            return Ok(response);
        }

        [HttpPost("changePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePassword)
        {
            Infraestructure.Entity.Entities.Users.User user = await _userService.ChangePassword(changePassword);

            var response = new ResponseDto()
            {
                IsSuccess = true,
                Message = user != null ? "Contraseña actualizada" : "Contraseña no actualizada",
                Result = user.Id
            };

            return Ok(response);
        }

        [HttpPost("verifyEmail")]
        [AllowAnonymous]
        public ActionResult VerifyEmail([FromBody] EmailRequest emailRequest)
        {
            Infraestructure.Entity.Entities.Users.User result = _userService.VerifyEmail(emailRequest.email);
            return Ok(new { value = result.Id });
        }

        [HttpPost("createProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProfile([FromBody] AddProfile_Dto profile)
        {
            var result = await _userService.CreateProfile(profile);
            return Ok(result);
        }

        [HttpPost("changeRole")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult ChangeRole([FromBody] ChangeRoleRequest changeRoleRequest)
        {
            var res = _userService.ChangeRole(changeRoleRequest.idUser, changeRoleRequest.idRole);
            if (res.GetType() == typeof(Infraestructure.Entity.Entities.Users.User))
                return Ok(new { value = res });
            return BadRequest(res);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _userService.Delete(idRequest.id);
            if (res.Result.GetType() == typeof(Infraestructure.Entity.Entities.Users.User))
                return Ok(new { value = idRequest.id });
            return BadRequest(res.Result);
        }

        #endregion
    }
}
