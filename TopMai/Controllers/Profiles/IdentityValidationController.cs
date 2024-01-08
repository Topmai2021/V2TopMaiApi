using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Profiles
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class IdentityValidationController : ControllerBase
    {
        #region Attributes
        private readonly IIdentityValidationService _identityValidationService;
        #endregion

        #region Builder
        public IdentityValidationController(IIdentityValidationService identityValidationService)
        {
            _identityValidationService = identityValidationService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        [AllowAnonymous]
        public ActionResult Get()
        {
            return Ok(_identityValidationService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_identityValidationService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] IdentityValidation identityValidation)
        {
            var res = _identityValidationService.Post(identityValidation);
            if (res.Result.GetType() == typeof(IdentityValidation))
                return Ok(new { value = res.Result});

            return BadRequest(res.Result);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Employee)]
        public ActionResult Put([FromBody] IdentityValidation identityValidation)
        {
            var res = _identityValidationService.Put(identityValidation);
            if (res.Result.GetType() == typeof(IdentityValidation))
                return Ok(new { value = res.Result});

            return BadRequest(res.Result);
        }

        [HttpPost("addImageToIdentityValidation")]
        public ActionResult AddImageToIdentityValidation([FromBody] ImageIdentityValidationRequest addImageToIdentityValidationRequest)
        {
            var res = _identityValidationService
                .AddImageToIdentityValidation(addImageToIdentityValidationRequest.idImage, addImageToIdentityValidationRequest.idIdentityValidation, addImageToIdentityValidationRequest.type);
            if (res.Result.GetType() == typeof(IdentityValidationImage))
                return Ok(new { value = res.Result });

            return BadRequest(res.Result);
        }

        [HttpPost("removeImagesIdentityValidation")]

        public ActionResult RemoveImagesIdentityValidation([FromBody] IdRequest idRequest)
        {
            var res = _identityValidationService.RemoveImagesIdentityValidation(idRequest.id);
            if (res.Result.GetType() == typeof(bool))
                return Ok(new { value = res.Result });

            return BadRequest(res.Result);
        }

        [HttpPost("hasPendingIdentityValidation")]
        public ActionResult HasPendingIdentityValidation([FromBody] IdRequest idRequest)
        {
            var res = _identityValidationService.HasPendingIdentityValidation(idRequest.id);
            if (res.Result.GetType() == typeof(bool))
                return Ok(new { value = res.Result });

            return BadRequest(res.Result);
        }
        [HttpPost("getImagesByIdentityValidation")]
        public ActionResult GetImagesByIdentityValidation([FromBody] IdRequest idRequest)
        {
            return Ok(_identityValidationService.GetImagesByIdentityValidation(idRequest.id));
        }

        #endregion
    }
}
