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
    public class BankController : ControllerBase
    {
        #region Attributes
        private readonly IBankService _bankService;
        #endregion

        #region Buil.der
        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._bankService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_bankService.Get(idRequest.id));
        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Post([FromBody] Bank bank)
        {
            var res = _bankService.Post(bank);
            if (res.Result.GetType() == typeof(Bank))
                return Ok(new { id = bank.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] Bank bank)
        {
            var res = _bankService.Put(bank);
            if (res.Result.GetType() == typeof(Bank))
                return Ok(new { value = bank.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _bankService.Delete(idRequest.id);
            if (res.Result.GetType() == typeof(Bank))
                return Ok();

            return BadRequest(res);
        }

        #endregion
    }
}
