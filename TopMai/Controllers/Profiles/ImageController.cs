using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO.Transactions.RechargueWallet;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Profiles
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ImageController : ControllerBase
    {
        #region Attributes
        private readonly IImageService _imageService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        #endregion

        #region Builder
        public ImageController(IImageService imageService, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _imageService = imageService;
            _env = env;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(this._imageService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_imageService.Get(idRequest.id));
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public ActionResult Post([FromBody] Image image)
        {
            var res = _imageService.Post(image);
            if (res.GetType() == typeof(Image))
                return Ok(new { value = image.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Image image)
        {
            var res = _imageService.Put(image);
            if (res.GetType() == typeof(Image))
                return Ok(new { value = image.Id.ToString() });

            return BadRequest(res);
        }


        [HttpPost("uploadPDF")]
        public ActionResult UploadPDF([FromQuery] string id, [FromForm] IFormFile file)
        {
            var res = _imageService.UploadPDF(id, file);
            return Ok(new { value = res });
        }

        [HttpGet("readPDF")]
        public ActionResult ReadPDF([FromQuery] string path)
        {
            var res = _imageService.ReadPDF(path, _env);
            return PhysicalFile(res.filePath, res.imageType);
        }

        [HttpGet("readImage")]
        [AllowAnonymous]
        public ActionResult read([FromQuery] string path)
        {
            var res = _imageService.ReadImage(path, _env);

            return PhysicalFile(res.filePath, res.imageType);
        }

        [HttpPost("uploadImageFile")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadImageFile([FromQuery] string id, [FromForm] IFormFile file)
        {
            var res = await _imageService.UploadImageFile(id, file, _env);
            return Ok(new { value = res });
        }

        [HttpPost("uploadImageFile2")]
        [AllowAnonymous]
        public async Task<IActionResult> UploadImageFile2([FromForm] ConfirmPaymentReference_Dto dto)
        {
            var res =await _imageService.UploadImageFile(dto.PaymentReference, dto.Evidencia, _env);
            return Ok(new { value = res });
        }


        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _imageService.Delete(idRequest.id);
            if (res.GetType() == typeof(Image))
                return Ok();

            return BadRequest(res);
        }

        #endregion

    }
}
