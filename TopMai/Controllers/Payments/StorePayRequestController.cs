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
    public class StorePayRequestController : ControllerBase
    {
        #region Attributes
        private readonly IStorePayRequestService _storePayRequestService;
        #endregion

        #region Buil.der
        public StorePayRequestController(IStorePayRequestService storePayRequestService)
        {
            _storePayRequestService = storePayRequestService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._storePayRequestService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_storePayRequestService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] StorePayRequest storePayRequest)
        {
            var res = _storePayRequestService.Post(storePayRequest);
            if (res.Result.GetType() == typeof(StorePayRequest))
                return Ok(new { id = storePayRequest.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] StorePayRequest storePayRequest)
        {
            var res = _storePayRequestService.Put(storePayRequest);
            if (res.Result.GetType() == typeof(StorePayRequest))
                return Ok(new { value = storePayRequest.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _storePayRequestService.Delete(idRequest.id);
            if (res.Result.GetType() == typeof(StorePayRequest))
                return Ok();

            return BadRequest(res);
        }

        [HttpPost("getStorePayRequestsByProfile")]
        public ActionResult GetStorePayRequestsByProfile([FromBody] IdRequest idRequest)
        {
            return Ok(_storePayRequestService.GetStorePayRequestsByProfile(idRequest.id));
        }
        

        #endregion
    }
}
