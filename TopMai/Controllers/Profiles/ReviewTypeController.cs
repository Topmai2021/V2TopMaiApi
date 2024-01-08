using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Profiles
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ReviewTypeController : ControllerBase
    {
        #region Attributes
        private readonly IReviewTypeService _reviewTypeService;
        #endregion

        #region Builder
        public ReviewTypeController(IReviewTypeService reviewTypeService)
        {
            _reviewTypeService = reviewTypeService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._reviewTypeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {

            return Ok(_reviewTypeService.Get(idRequest.id));

        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] ReviewType reviewType)
        {
            var res = _reviewTypeService.Post(reviewType);
            if (res.GetType() == typeof(ReviewType))
                return Ok(new { id = reviewType.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] ReviewType reviewType)
        {
            var res = _reviewTypeService.Put(reviewType);
            if (res.GetType() == typeof(ReviewType))
                return Ok(new { value = reviewType.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _reviewTypeService.Delete(idRequest.id);
            if (res.GetType() == typeof(ReviewType))
                return Ok();

            return BadRequest(res);
        }

        #endregion
    }
}
