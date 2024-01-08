using Common.Utils.Helpers;
using Infraestructure.Entity.DTOs;
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
    public class SellRequestController : ControllerBase
    {
        #region Attributes
        private readonly ISellRequestService _sellRequestService;
        #endregion

        #region Builder
        public SellRequestController(ISellRequestService sellRequestService)
        {
            _sellRequestService = sellRequestService;
        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._sellRequestService.GetAll());
        }

        [HttpGet("get/{id}")]
        public ActionResult Get(Guid id)
        {
            /**
            var token = Request.Headers["Authorization"];
           
            if (Helper.IsAdmin(token))
                return Ok(this._sellRequestService.GetAll());
            */
            return Ok(_sellRequestService.Get(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] SellRequestDTO status)
        {
            var res = await _sellRequestService.Post(status);
            return Ok(new { id = status.Id.ToString() });
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Put([FromBody] SellRequest status)
        {
            var res = await _sellRequestService.Put(status);
            return Ok(new { value = status.Id.ToString() });
        }

        [HttpPost("confirmSellOffer")]
        public ActionResult ConfirmSellOffer([FromBody] SellOfferRequest sellOfferRequest)
        {
            var res = _sellRequestService.ConfirmSellOffer(sellOfferRequest);
            return Ok(res);
        }

        [HttpPost("acceptSellOffer")]
        public ActionResult AcceptSellOffer([FromBody] SellOfferRequest sellOfferRequest)
        {
            var res = _sellRequestService.AcceptSellOffer(sellOfferRequest.sellRequestId, sellOfferRequest.userId);
            if (res.GetType() == typeof(SellRequest))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("declineSellOffer")]
        public ActionResult DeclineSellOffer([FromBody] SellOfferRequest sellOfferRequest)
        {
            var res = _sellRequestService.DeclineSellOffer(sellOfferRequest.sellRequestId, sellOfferRequest.userId);
            if (res.GetType() == typeof(SellRequest))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("getBuyOffersByUserId")]
        public ActionResult GetBuyOffersByUserId([FromBody] IdRequest idRequest)
        {
            return Ok(_sellRequestService.GetBuyOffersByUserId(idRequest.id));
        }

        [HttpPost("getSellOffersByUserId")]
        public ActionResult GetSellOffersByUserId([FromBody] IdRequest idRequest)
        {
            return Ok(_sellRequestService.GetSellOffersByUserId(idRequest.id));
        }

        [HttpPost("getComissions")]
        public ActionResult GetComissions([FromBody] PublicationRequest publicationRequest)
        {
            return Ok(_sellRequestService.GetComissions(publicationRequest.publicationId, publicationRequest.total, publicationRequest.withShippment));

        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _sellRequestService.Delete(idRequest.id);
            if (res.GetType() == typeof(SellRequest))
                return Ok();

            return BadRequest(res);
        }

        #endregion
    }
}
