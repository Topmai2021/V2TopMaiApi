using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Profiles
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class GenderController : ControllerBase
    {
        #region Attributes
        private readonly IGenderService _genderService;
        #endregion

        #region Builder
        public GenderController(IGenderService genderService)
        {
            this._genderService = genderService;
        }
        #endregion

        #region Services
        [HttpGet("getAll")]
        public ActionResult Get()
        {
            List<GenderDto> result = _genderService.GetAll();
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result,
            };

            return Ok(response);
        }

        [HttpGet("get")]
        public ActionResult Get(int idGender)
        {
            GenderDto result = _genderService.Get(idGender);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result,
            };

            return Ok(response);
        }
        [HttpDelete("delete")]
        public ActionResult Delete([FromBody] int id)
        {
            var res = _genderService.Delete(id);
            return Ok(res);
        }

        #endregion
    }
}
