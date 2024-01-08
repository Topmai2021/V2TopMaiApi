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
    public class PaymentMethodController : ControllerBase
    {
        #region Attributes
        private readonly IPaymentMethodService _paymentMethodService;
        #endregion

        #region Builder
        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(_paymentMethodService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] int idRequest)
        {
            var token = Request.Headers["Authorization"];

            if (Helper.IsAdmin(token))
                return Ok(this._paymentMethodService.GetAll());

            return Ok(_paymentMethodService.Get(idRequest));
        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Post([FromBody] PaymentMethod paymentMethod)
        {
            var res = await _paymentMethodService.Post(paymentMethod);
            return Ok(new { id = paymentMethod.Id.ToString() });

        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Put([FromBody] PaymentMethod paymentMethod)
        {
            var res = await _paymentMethodService.Put(paymentMethod);
            return Ok(new { value = paymentMethod.Id.ToString() });
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Delete([FromBody] int idRequest)
        {
            var res = await _paymentMethodService.Delete(idRequest);
            return Ok();
        }

        #endregion

    }
}
