using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.Services.Post.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Posts
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class PostController : ControllerBase
    {
        #region Attributes
        private readonly IPostService _postService;
        #endregion

        #region Builder
        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._postService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_postService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Post post)
        {
            var res = _postService.Post(post);
            if (res.GetType() == typeof(Post))
                return Ok(new { id = post.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Post post)
        {
            var res = _postService.Put(post);
            if (res.GetType() == typeof(Post))
                return Ok(new { value = post.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _postService.Delete(idRequest.id);
            if (res.GetType() == typeof(Post))
                return Ok();

            return BadRequest(res);
        }

        [HttpPost("getPostByProfileId")]

        public ActionResult GetPostByProfile([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);

            var res = _postService.GetPostByProfileId(idRequest.id, Guid.Parse(idUser));
            if (res.GetType() == typeof(List<Post>)) return Ok(res);
            return BadRequest(res);
        }


        [HttpPost("addImageToPost")]
        public ActionResult AddImageToPost([FromBody] PostImageRequest postImageRequest)
        {
            var res = _postService.AddImageToPost(postImageRequest.idImage, postImageRequest.idPost);
            if (res.GetType() == typeof(Post))
            {
                Post p = (Post)res;
                return Ok(new { value = p.Id });
            }

            return BadRequest(res);
        }


        [HttpGet("getContactsPostByProfileId")]
        public ActionResult GetContactsPostByProfileId(Guid idProfile)
        {
            var res = _postService.GetContactsPostByProfileId(idProfile);

            var response = new ResponseDto()
            {
                IsSuccess = res != null,
                Message = res== null ? "No existe Contactos" : "Contactos",
                Result = res,
                
            };

            return Ok(response);
        }

        #endregion
    }
}
