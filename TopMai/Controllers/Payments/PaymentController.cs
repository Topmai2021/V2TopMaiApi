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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;

        }

        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Get()
        {
        

            return Ok(_paymentService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_paymentService.Get(idRequest.id));
        }


        [HttpPost("getPaymentsByUserId")]
        public ActionResult GetPaymentsByUser([FromBody] IdRequest idRequest)
        {
            return Ok(_paymentService.GetPaymentsByUser(idRequest.id));
        }

        /**
                [HttpPost("create")]          
                public ActionResult Post([FromBody] Payment payment)
                {
                    try
                    {
                        var res = _paymentService.Post(payment);
                        if (res.GetType() == typeof(Payment)) return Ok(new {id= payment.Id.ToString()});
                        return BadRequest(res);
                    }
                    catch(Exception e)
                    {

                        return BadRequest(e);

                    }
              
                }

                [HttpPost("edit")]        
                public ActionResult Put( [FromBody] Payment payment)
                {
                    try
                    {
                        var res = _paymentService.Put(payment);
                        if (res.GetType() == typeof(Payment)) return Ok(new { value = payment.Id.ToString() });
                        return BadRequest(res);

                    }
                    catch (Exception e)
                    {
                        return BadRequest(e);
                    }

                }

                [HttpPost("delete")]

                public ActionResult Delete ( [FromBody] IdRequest idRequest)
                {
                    try
                    {
                        var res = _paymentService.Delete(idRequest.id);
                        if (res.GetType() == typeof(Payment)) return Ok();
                        return BadRequest(res);
                    }
                    catch (Exception e)
                    {

                        return BadRequest(e);

                    }
                }
            **/

    }
}
