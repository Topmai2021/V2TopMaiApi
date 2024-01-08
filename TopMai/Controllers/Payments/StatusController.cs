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
    public class StatusController : ControllerBase
    {
        #region Attributes
        private readonly IStatusService _statusService;

        //public object Herlper { get; private set; }
        #endregion
        
        #region Builder
        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._statusService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] int id)
        {
            var token = Request.Headers["Authorization"];
            if (Helper.IsAdmin(token))
                return Ok(_statusService.GetAll());

            return Ok(_statusService.Get(id));

        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Post([FromBody] Status status)
        {
            var res =await  _statusService.Post(status);
            return Ok(new { id = status.Id.ToString() });
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Put([FromBody] Status status)
        {
            var res = await _statusService.Put(status);
            return Ok(new { value = status.Id.ToString() });
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var res = await _statusService.Delete(id);
            return Ok(res);
        }
        #endregion

    }
}
