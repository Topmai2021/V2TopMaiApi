using Common.Utils.Resources;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.PublicationComment;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Products
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class PublicationCommentController : ControllerBase
    {
        #region Attributes
        private readonly IPublicationCommentService _publicationCommentService;
        #endregion

        #region Builder
        public PublicationCommentController(IPublicationCommentService publicationCommentService)
        {
            _publicationCommentService = publicationCommentService;
        }
        #endregion

        #region Services
        [HttpPost("get")]
        public ActionResult Get(Guid idPublicationComment)
        {
            var result = _publicationCommentService.GetPublicationComment(idPublicationComment);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Error = string.Empty,
                Result = result
            };

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] PublicationCommentDTO publicationComment)
        {
            IActionResult action;

            bool result = await _publicationCommentService.Post(publicationComment);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemInserted : GeneralMessages.ItemNoInserted,
                Error = result ? GeneralMessages.ItemInserted : GeneralMessages.ItemNoInserted,
                Result = result
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Put([FromBody] PublicationCommentDTO publicationComment)
        {
            IActionResult action;

            bool result = await _publicationCommentService.Put(publicationComment);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemUpdated : GeneralMessages.ItemNoUpdated,
                Error = result ? GeneralMessages.ItemUpdated : GeneralMessages.ItemNoUpdated,
                Result = result
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }


        [HttpPost("getPublicationCommentsByPublication")]
        [AllowAnonymous]
        public ActionResult GetPublicationCommentsByPublication([FromBody] IdRequest idRequest)
        {
            var res = _publicationCommentService.GetPublicationCommentsByPublication(idRequest.id);
            if (res.GetType() == typeof(List<PublicationComment>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid idPublicationComment)
        {
            IActionResult action;

            bool result = await _publicationCommentService.Delete(idPublicationComment);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted,
                Error = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted,
                Result = result
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        #endregion

    }
}
