using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Complaints;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Complaints.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Complaints
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ReasonController : ControllerBase
    {
        #region Attributes
        private readonly IReasonService _reasonService;
        #endregion

        #region Builder
        public ReasonController(IReasonService reasonService)
        {
            _reasonService = reasonService;
        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._reasonService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];

            if (Helper.IsAdmin(token))
                return Ok(this._reasonService.GetAll());

            return Ok(_reasonService.Get(idRequest.id));
        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Post([FromBody] Reason reason)
        {
            var res = _reasonService.Post(reason);
            if (res.GetType() == typeof(Reason))
            {
                Reason reasonRes = (Reason)res;
                return Ok(new { id = reasonRes.Id.ToString() });
            }
            return BadRequest(res);

        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] Reason reason)
        {
            var res = _reasonService.Put(reason);
            if (res.GetType() == typeof(Reason))
                return Ok(new { value = reason.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _reasonService.Delete(idRequest.id);
            if (res.GetType() == typeof(Reason))
                return Ok();

            return BadRequest(res);
        }

        #endregion

    }
}
