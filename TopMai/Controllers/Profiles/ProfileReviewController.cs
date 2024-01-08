using Common.Utils.Resources;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Profiles
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ProfileReviewController : ControllerBase
    {
        #region Attributes
        private readonly IProfileReviewService _profileReviewService;
        #endregion

        #region Builder
        public ProfileReviewController(IProfileReviewService profileReviewService)
        {
            this._profileReviewService = profileReviewService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._profileReviewService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_profileReviewService.Get(idRequest.id));
        }

        [HttpPost("getProfileReviewsByProfileId")]
        [AllowAnonymous]
        public ActionResult GetProfileReviewsByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _profileReviewService.GetProfileReviewsByProfileId(idRequest.id);
            if (res.GetType() == typeof(List<ProfileReview>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("GetValorationByProfileId")]
        [AllowAnonymous]

        public ActionResult GetValorationByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _profileReviewService.GetValorationByProfileId(idRequest.id);
            if ((res.GetType() == typeof(List<object>) || res.GetType() == typeof(int)))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("GetMyValorationToProfileId")]
        public ActionResult GetMyValorationToProfileId([FromBody] FromToRequest fromToRequest)
        {
            var res = _profileReviewService.GetMyValorationToProfileId(fromToRequest.FromId, fromToRequest.ToId);
            if ((res.GetType() == typeof(List<object>) || res.GetType() == typeof(int)))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("isCalificated")]
        public ActionResult IsCalificated([FromBody] ProfileReview profileReview)
        {
            var res = _profileReviewService.IsCalificated(profileReview.FromId, profileReview.ToId, profileReview.SellId);
            if (res.GetType() == typeof(bool))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] ProfileReview profileReview)
        {
            var res = _profileReviewService.Post(profileReview);
            if (res.GetType() == typeof(ProfileReview))
                return Ok(new { id = profileReview.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] ProfileReview profileReview)
        {
            var res = _profileReviewService.Put(profileReview);
            if (res.GetType() == typeof(ProfileReview))
                return Ok(new { value = profileReview.Id.ToString() });

            return BadRequest(res);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid idProfileReview)
        {
            IActionResult action;
            bool result = await _profileReviewService.Delete(idProfileReview);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted,
                Error = result ? string.Empty : GeneralMessages.ItemNoDeleted,
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
