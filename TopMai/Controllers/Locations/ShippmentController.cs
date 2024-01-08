using Infraestructure.Entity.Entities.Locations;
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
    public class ShippmentController : ControllerBase
    {
        #region Attributes
        private readonly IShippmentService _shippmentService;
        #endregion

        #region Builder
        public ShippmentController(IShippmentService shippmentService)
        {
            _shippmentService = shippmentService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._shippmentService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_shippmentService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Shippment shippment)
        {
            var res = _shippmentService.Post(shippment);
            if (res.GetType() == typeof(Shippment))
                return Ok(new { id = shippment.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Shippment shippment)
        {
            var res = _shippmentService.Put(shippment);
            if (res.GetType() == typeof(Shippment))
                return Ok(new { value = shippment.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _shippmentService.Delete(idRequest.id);
            if (res.GetType() == typeof(Shippment))
                return Ok();

            return BadRequest(res);
        }

        #endregion

    }
}
