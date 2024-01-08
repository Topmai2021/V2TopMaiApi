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
    public class DevolutionController : ControllerBase
    {
        #region Attributes
        private readonly IDevolutionService _devolutionService;
        #endregion

        #region Builder
        public DevolutionController(IDevolutionService devolutionService)
        {
            _devolutionService = devolutionService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._devolutionService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_devolutionService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Devolution devolution)
        {
            var res = _devolutionService.Post(devolution);
            if (res.Result.GetType() == typeof(Devolution))
                return Ok(new { id = devolution.Id.ToString() });

            return BadRequest(res.Result);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Devolution devolution)
        {
            var res = _devolutionService.Put(devolution);
            if (res.GetType() == typeof(Devolution))
                return Ok(new { value = devolution.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("acceptDevolution")]
        
        public ActionResult AcceptDevolution([FromBody] ObjectStatusRequest objectStatusRequest)
        {
            var res = _devolutionService.AcceptDevolution(objectStatusRequest.id, (Guid)objectStatusRequest.userId);
            if (res.Result.GetType() == typeof(Devolution))
            {
                Devolution devolution = (Devolution)res.Result;
                return Ok(new { value = devolution.Id.ToString() });
            }

            return BadRequest(res.Result);
        }

        [HttpPost("declineDevolution")]

        public ActionResult DeclineDevolution([FromBody] ObjectStatusRequest objectStatusRequest)
        {
            var res = _devolutionService.DeclineDevolution(objectStatusRequest.id, (Guid)objectStatusRequest.userId);
            if (res.Result.GetType() == typeof(Devolution))
            {
                Devolution devolution = (Devolution)res.Result;
                return Ok(new { value = devolution.Id.ToString() });
            }

            return BadRequest(res.Result);
        }

        [HttpPost("checkDevolutionStatus")]

        public ActionResult CheckDevolutionStatus()
        {
            return Ok(_devolutionService.CheckDevolutionStatus());
        }

        [HttpPost("changeDevolutionStatus")]
        public ActionResult ChangeDevolutionStatus([FromBody] ObjectStatusRequest objectStatusRequest)
        {
            var res = _devolutionService.ChangeDevolutionStatus(objectStatusRequest.id, objectStatusRequest.statusId);
            if (res.Result.GetType() == typeof(Devolution))
                return Ok(res.Result);

            return BadRequest(res.Result);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _devolutionService.Delete(idRequest.id);
            if (res.GetType() == typeof(Devolution))
                return Ok();

            return BadRequest(res);
        }

        #endregion
    }
}
