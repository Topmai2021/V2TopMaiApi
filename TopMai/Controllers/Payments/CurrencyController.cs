using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Payments
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class CurrencyController : ControllerBase
    {
        #region Attributes
        private readonly ICurrencyService _currencyService;
        #endregion

        #region Buil.der
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._currencyService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] int idRequest)
        {
            return Ok(_currencyService.Get(idRequest));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] Currency currency)
        {
            var res = await _currencyService.Post(currency);
            return Ok(new { id = currency.Id.ToString() });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Put([FromBody] Currency currency)
        {
            var res = await _currencyService.Put(currency);
            return Ok(new { value = currency.Id.ToString() });
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] int idRequest)
        {
            var res = await _currencyService.Delete(idRequest);
            return Ok(res);
        }

        #endregion
    }
}
