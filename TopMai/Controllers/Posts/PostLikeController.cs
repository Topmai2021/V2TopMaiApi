using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Post.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Posts
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class PostLikeController : ControllerBase
    {
        #region Attributes
        private readonly IPostLikeService _postLikeService;
        #endregion

        #region Builder
        public PostLikeController(IPostLikeService postLikeService)
        {
            _postLikeService = postLikeService;
        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(this._postLikeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];
            if (Helper.IsAdmin(token))
                return Ok(this._postLikeService.GetAll());

            return Ok(_postLikeService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] PostLike postLike)
        {
            var res = _postLikeService.Post(postLike);
            if (res.GetType() == typeof(PostLike))
            {
                PostLike postLikeRes = (PostLike)res;
                return Ok(new { id = postLikeRes.Id.ToString() });
            }

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] PostLike postLike)
        {
            var res = _postLikeService.Put(postLike);
            if (res.GetType() == typeof(PostLike))
                return Ok(new { value = postLike.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _postLikeService.Delete(idRequest.id);
            if (res.GetType() == typeof(PostLike))
                return Ok();

            return BadRequest(res);
        }

        #endregion

    }
}
