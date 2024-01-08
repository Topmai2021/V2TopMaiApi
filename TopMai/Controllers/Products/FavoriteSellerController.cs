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
    public class FavoriteSellerController : ControllerBase
    {
        #region Attributes
        private readonly IFavoriteSellerService _favoriteSellerService;
        #endregion

        #region Builder
        public FavoriteSellerController(IFavoriteSellerService favoriteSellerService)
        {
            this._favoriteSellerService = favoriteSellerService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._favoriteSellerService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_favoriteSellerService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] FavoriteSeller favoriteSeller)
        {
            var res = _favoriteSellerService.Post(favoriteSeller);
            if (res.GetType() == typeof(FavoriteSeller))
                return Ok(new { value = favoriteSeller.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] FavoriteSeller favoriteSeller)
        {
            var res = _favoriteSellerService.Put(favoriteSeller);
            if (res.GetType() == typeof(FavoriteSeller))
                return Ok(new { value = favoriteSeller.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _favoriteSellerService.Delete(idRequest.id);
            if (res.GetType() == typeof(FavoriteSeller))
                return Ok();

            return BadRequest(res);
        }

        [HttpPost("getFavoriteSellersByProfileId")]
        public ActionResult GetFavoriteSellersByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _favoriteSellerService.GetFavoriteSellersByProfileId(idRequest.id);
            if (res.GetType() == typeof(List<FavoriteSeller>))
                return Ok(res);

            return BadRequest(res);
        }

        #endregion
    }
}
