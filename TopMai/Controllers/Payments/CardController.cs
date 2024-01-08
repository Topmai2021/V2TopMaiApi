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
    public class CardController : ControllerBase
    {
        #region Attributes
        private readonly ICardService _cardService;
        #endregion

        #region Buil.der
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._cardService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_cardService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Card card)
        {
            var res = _cardService.Post(card);
            if (res.Result.GetType() == typeof(Card))
                return Ok(new { id = card.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Card card)
        {
            var res = _cardService.Put(card);
            if (res.Result.GetType() == typeof(Card))
                return Ok(new { value = card.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _cardService.Delete(idRequest.id);
            if (res.Result.GetType() == typeof(Card))
                return Ok();

            return BadRequest(res);
        }

        [HttpPost("getCardsByProfile")]
        public ActionResult GetCardsByProfile([FromBody] IdRequest idRequest)
        {
            return Ok(_cardService.GetCardsByProfile(idRequest.id));
        }
        

        #endregion
    }
}
