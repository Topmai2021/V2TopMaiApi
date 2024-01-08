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
    public class SellController : ControllerBase
    {
        #region Attributes
        private readonly ISellService _sellService;
        #endregion

        #region Builder
        public SellController(ISellService sellService)
        {
            _sellService = sellService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        public async Task<IActionResult> Get([FromQuery]int page = 1, int limit=100000000)
        {
            var sells =  _sellService.GetAll(page, limit);
            return Ok(sells);
        }



        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_sellService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] Sell sell, [FromQuery] int paymentMethodId)
        {
            var res = await _sellService.Post(sell, paymentMethodId);
            return Ok(new { id = res.Id.ToString() });
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Sell sell)
        {
            var res = _sellService.Put(sell);
            if (res.GetType() == typeof(Sell))
                return Ok(new { value = sell.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _sellService.Delete(idRequest.id);
            if (res.GetType() == typeof(Sell))
                return Ok();

            return BadRequest(res);
        }

        [HttpPost("getSellsByProfileId")]
        public ActionResult GetSellsByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _sellService.GetSellsByProfileId(idRequest.id);
            if (res.GetType() == typeof(List<Sell>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("getBuysByProfileId")]
        public ActionResult GetBuysByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _sellService.GetBuysByProfileId(idRequest.id);
            if (res.GetType() == typeof(List<Sell>))
                return Ok(res);

            return BadRequest(res);
        }


        [HttpPost("getAmountOfSellsByProfileId")]
        public ActionResult GetAmountOfSellsByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _sellService.GetAmountOfSellsByProfileId(idRequest.id);
            if (res.GetType() == typeof(int))
                return Ok(new { value = res });

            return BadRequest(res);
        }

        [HttpPost("getAmountOfBuysByProfileId")]
        public ActionResult GetAmountOfBuysByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _sellService.GetAmountOfBuysByProfileId(idRequest.id);
            if (res.GetType() == typeof(int))
                return Ok(new { value = res });

            return BadRequest(res);
        }

        [HttpPost("getAmountOfShippmentsByProfileId")]
        public ActionResult GetAmountOfShippmentsByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _sellService.GetAmountOfShippmentsByProfileId(idRequest.id);
            if (res.GetType() == typeof(int))
                return Ok(new { value = res });

            return BadRequest(res);
        }


        [HttpPost("getTracker")]
        public ActionResult GetTracker([FromBody] TrackerRequest trackerRequest)
        {
            var res = _sellService.GetTracker(trackerRequest.trackingCode);
            return Ok(res);
        }

        [HttpPost("createTracker")]
        //carrier
        //tracking_code
        public ActionResult CreateTracker([FromBody] TrackerRequest trackerRequest)
        {
            var res = _sellService.CreateTracker(trackerRequest.carrier, trackerRequest.trackingCode);
            return Ok(res.Result);
        }
        [HttpPost("changeSellStatus")]
        public ActionResult ChangeSellStatus([FromBody] ObjectStatusRequest objectStatusRequest)
        {
            var res = _sellService.ChangeSellStatus(objectStatusRequest.id, objectStatusRequest.statusId);

            if (res.GetType() == typeof(Sell))
            {
                Sell sell = (Sell)res;
                return Ok(new { value = sell.Id.ToString() });

            }

            return BadRequest(res);
        }

        #endregion
    }
}
