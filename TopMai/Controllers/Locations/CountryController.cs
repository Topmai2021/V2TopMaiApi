using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Locations.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Locations
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class CountryController : ControllerBase
    {
        #region Attributes
        private readonly ICountryService _countryService;
        #endregion

        #region Builder
        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        #endregion

        #region Services
        [HttpGet("getAll")]
        public ActionResult Get()
        {
            return Ok(_countryService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_countryService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Country country)
        {
            var res = _countryService.Post(country);
            if (res.GetType() == typeof(Country))
                return Ok(new { value = country.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Country country)
        {
            var res = _countryService.Put(country);
            if (res.GetType() == typeof(Country))
                return Ok(new { value = country.Id.ToString() });

            return BadRequest(res);
        }
        [HttpDelete("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _countryService.Delete(idRequest.id);
            if (res.GetType() == typeof(Gender))
                return Ok();

            return BadRequest(res);
        }
        #endregion
    }
}
