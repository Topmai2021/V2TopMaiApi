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
    public class FavoritePublicationController : ControllerBase
    {
        #region Attributes
        private readonly IFavoritePublicationService _favoritePublicationService;
        #endregion

        #region Builder
        public FavoritePublicationController(IFavoritePublicationService favoritePublicationService)
        {
            this._favoritePublicationService = favoritePublicationService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._favoritePublicationService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_favoritePublicationService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] FavoritePublication favoritePublication)
        {
            var res = _favoritePublicationService.Post(favoritePublication);
            if (res.GetType() == typeof(FavoritePublication))
                return Ok(new { id = favoritePublication.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] FavoritePublication favoritePublication)
        {
            var res = _favoritePublicationService.Put(favoritePublication);
            if (res.GetType() == typeof(FavoritePublication))
                return Ok(new { value = favoritePublication.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _favoritePublicationService.Delete(idRequest.id);
            if (res.GetType() == typeof(FavoritePublication))
                return Ok();

            return BadRequest(res);
        }


        [HttpPost("getFavoritePublicationsByProfileId")]
        public ActionResult GetFavoritePublicationsByProfileId([FromBody] IdRequest idRequest)
        {
            return Ok(_favoritePublicationService.GetFavoritePublicationsByProfile(idRequest.id));
        }

        #endregion

    }
}
