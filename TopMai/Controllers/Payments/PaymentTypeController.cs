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
    public class PaymentTypeController : ControllerBase
    {
        #region Attributes
        private readonly IPaymentTypeService _paymentTypeService;
        #endregion

        #region Builder
        public PaymentTypeController(IPaymentTypeService paymentTypeService)
        {
            this._paymentTypeService = paymentTypeService;
        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(_paymentTypeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];
            if (Helper.IsAdmin(token))
                return Ok(this._paymentTypeService.GetAll());

            return Ok(_paymentTypeService.Get(idRequest.id));
        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Post([FromBody] PaymentType paymentType)
        {
            var res = _paymentTypeService.Post(paymentType);
            if (res.GetType() == typeof(PaymentType))
                return Ok(new { id = paymentType.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] PaymentType paymentType)
        {
            var res = _paymentTypeService.Put(paymentType);
            if (res.GetType() == typeof(PaymentType))
                return Ok(new { value = paymentType.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _paymentTypeService.Delete(idRequest.id);
            if (res.GetType() == typeof(PaymentType))
                return Ok();

            return BadRequest(res);
        }


        #endregion
    }
}
