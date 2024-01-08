using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Payments
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class StatusChangeController : ControllerBase
    {
        #region Attributes
        private readonly IStatusChangeService _StatusChangeService;

        #endregion
        #region Builder
        public StatusChangeController(IStatusChangeService StatusChangeService)
        {
            _StatusChangeService = StatusChangeService;
        }
        #endregion

        #region Services

        
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._StatusChangeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];
            if (Helper.IsAdmin(token))
                return Ok(_StatusChangeService.GetAll());

            return Ok(_StatusChangeService.Get(idRequest.id));

        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Post([FromBody] StatusChange StatusChange)
        {
            var res = _StatusChangeService.Post(StatusChange);
            if (res.GetType() == typeof(StatusChange))
                return Ok(new { id = StatusChange.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] StatusChange StatusChange)
        {
            var res = _StatusChangeService.Put(StatusChange);
            if (res.GetType() == typeof(StatusChange))
                return Ok(new { value = StatusChange.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _StatusChangeService.Delete(idRequest.id);
            if (res.GetType() == typeof(StatusChange))
                return Ok();

            return BadRequest(res);
        }

        #endregion

    }
}
