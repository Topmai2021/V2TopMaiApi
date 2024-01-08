using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class AttributeController : ControllerBase
    {
        #region Attributes
        private readonly IAttributeService _attributeService;
        #endregion

        #region Builder
        public AttributeController(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._attributeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_attributeService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Infraestructure.Entity.Entities.Products.Attribute attribute)
        {
            var res = _attributeService.Post(attribute);
            if (res.GetType() == typeof(Infraestructure.Entity.Entities.Products.Attribute))
                return Ok(new { id = attribute.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Infraestructure.Entity.Entities.Products.Attribute attribute)
        {
            var res = _attributeService.Put(attribute);
            if (res.GetType() == typeof(Infraestructure.Entity.Entities.Products.Attribute))
                return Ok(new { value = attribute.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _attributeService.Delete(idRequest.id);
            if (res.GetType() == typeof(Infraestructure.Entity.Entities.Products.Attribute))
                return Ok();

            return BadRequest(res);
        }



        [HttpPost("getAttributesByCategoryId")]
        public ActionResult GetAttributesByCategoryId( [FromBody] IdRequest idRequest)
        {
            try
            {
                var res = _attributeService.GetAttributesByCategoryId(idRequest.id);
                if (res.Result.GetType() == typeof(List<Infraestructure.Entity.Entities.Products.Attribute>)) return Ok(res.Result);
                return BadRequest(res);
            }
            catch (Exception e)
            {

                return BadRequest(e);

            }
        }

        #endregion
    }
}
