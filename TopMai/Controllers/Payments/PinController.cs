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
    public class PinController : ControllerBase
    {
        #region Attributes
        private readonly IPinService _pinService;
        #endregion

        #region Buil.der
        public PinController(IPinService pinService)
        {
            _pinService = pinService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._pinService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_pinService.Get(idRequest.id));
        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Post([FromBody] Pin pin)
        {
            var res = _pinService.Post(pin);
            if (res.Result.GetType() == typeof(Pin))
                return Ok(new { id = pin.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] Pin pin)
        {
            var res = _pinService.Put(pin);
            if (res.Result.GetType() == typeof(Pin))
                return Ok(new { value = pin.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("validatePin")]
        public ActionResult ValidatePin([FromBody] Pin pin)
        {
            var res = _pinService.ValidatePin((Guid)pin.UserId,pin.Value);
            if (res.GetType() == typeof(bool))
                return Ok(res);

            return BadRequest(res);
        }



        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _pinService.Delete(idRequest.id);
            if (res.Result.GetType() == typeof(Pin))
                return Ok();

            return BadRequest(res);
        }

        #endregion
    }
}
