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
    public class PostCommentController : ControllerBase
    {
        #region Attributes
        private readonly IPostCommentService _postCommentService;
        #endregion

        #region Builder
        public PostCommentController(IPostCommentService postCommentService)
        {
            _postCommentService = postCommentService;
        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(this._postCommentService.GetAll());
        }

        [HttpPost("get")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];
            if (Helper.IsAdmin(token))
                return Ok(this._postCommentService.GetAll());

            return Ok(_postCommentService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] PostComment postComment)
        {
            var res = _postCommentService.Post(postComment);
            if (res.GetType() == typeof(PostComment))
            {
                PostComment postCommentRes = (PostComment)res;
                return Ok(new { id = postCommentRes.Id.ToString() });
            }

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] PostComment postComment)
        {
            var res = _postCommentService.Put(postComment);
            if (res.GetType() == typeof(PostComment))
                return Ok(new { value = postComment.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _postCommentService.Delete(idRequest.id);
            if (res.GetType() == typeof(PostComment))
                return Ok();

            return BadRequest(res);
        } 
        #endregion

    }
}
