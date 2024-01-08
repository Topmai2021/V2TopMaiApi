using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Products
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ShippmentTypeController : ControllerBase
    {
        #region Attributes
        private readonly IShippmentTypeService _shippmentTypeService;
        #endregion

        #region Builder
        public ShippmentTypeController(IShippmentTypeService shippmentTypeService)
        {
            this._shippmentTypeService = shippmentTypeService;

        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(_shippmentTypeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdIntRequest idIntRequest)
        {
            return Ok(_shippmentTypeService.Get(idIntRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] ShippmentType shippmentType)
        {
            var res = _shippmentTypeService.Post(shippmentType);
            if (res.GetType() == typeof(ShippmentType))
                return Ok(new { value = shippmentType.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] ShippmentType shippmentType)
        {
            var res = _shippmentTypeService.Put(shippmentType);
            if (res.GetType() == typeof(ShippmentType))
                return Ok(new { value = shippmentType.Id.ToString() });

            return BadRequest(res);
        }

        #endregion
    }
}
