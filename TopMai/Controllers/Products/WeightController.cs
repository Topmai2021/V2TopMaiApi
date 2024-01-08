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
    public class WeightController : ControllerBase
    {
        #region Attributes
        private readonly IWeightService _weightService;
        #endregion

        #region Builder
        public WeightController(IWeightService weightService)
        {
            this._weightService = weightService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._weightService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_weightService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Weight weight)
        {
            var res = _weightService.Post(weight);
            if (res.GetType() == typeof(Weight))
                return Ok(new { id = weight.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Weight weight)
        {
            var res = _weightService.Put(weight);
            if (res.GetType() == typeof(Weight))
                return Ok(new { value = weight.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _weightService.Delete(idRequest.id);
            if (res.GetType() == typeof(Weight))
                return Ok();

            return BadRequest(res);
        }

        #endregion
    }
}
