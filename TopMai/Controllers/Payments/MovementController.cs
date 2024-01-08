using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Payments
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class MovementController : ControllerBase
    {
        #region Attributes
        private readonly IMovementService _movementService;
        #endregion

        #region Builder
        public MovementController(IMovementService movementService)
        {
            _movementService = movementService;
        }
        #endregion

        #region Services
        [HttpGet("getAll")]
        [CustomRolFilterImplement(Roles.Employee)]
        public async Task<ActionResult> Get([FromQuery] int page, int limit)
        {
            var Movements = await this._movementService.GetAll(page, limit);

            
            return Ok(Movements);
        }


        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_movementService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Movement movement)
        {
            var res = _movementService.Post(movement);
            return Ok(new { id = movement.Id.ToString() });
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Employee)]
        public async Task<IActionResult> Put([FromBody] Movement movement)
        {
            var res = await _movementService.Put(movement);
            return Ok(new { value = movement.Id.ToString() });
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]

        public async Task<IActionResult> Delete([FromBody] IdRequest idRequest)
        {
            var res = await _movementService.Delete(idRequest.id);
            return Ok(res);
        }

        [HttpPost("getPendingInputByUserId")]
        public ActionResult GetPendingInputByUserId([FromBody] IdRequest idRequest)
        {
            var res = _movementService.GetPendingInputByUserId(idRequest.id);
            return Ok(res);
        }

        [HttpPost("getAllMovementsByUserId")]
        public ActionResult GetAllMovementsByUserId([FromBody] IdRequest idRequest)
        {
            var res = _movementService.GetAllMovementsByUserId(idRequest.id);
            return Ok(res);
        }

        [HttpPost("getPendingOutputByUserId")]
        public ActionResult GetPendingOutputByUserId([FromBody] IdRequest idRequest)
        {
            var res = _movementService.GetPendingOutputByUserId(idRequest.id);
            return Ok(res);
        }

        [HttpPost("getSolvedInputsByUserId")]
        public ActionResult GetSolvedInputsByUserId([FromBody] IdRequest idRequest)
        {
            var res = _movementService.GetSolvedInputsByUserId(idRequest.id);
            return Ok(res);
        }

        [HttpPost("getSolvedOutputsByUserId")]
        public ActionResult GetSolvedOutputsByUserId([FromBody] IdRequest idRequest)
        {

            var res = _movementService.GetSolvedOutputsByUserId(idRequest.id);
            return Ok(res);
        }

        [HttpPost("getAmountOfPendingMovementInputs")]
        public ActionResult GetAmountOfPendingMovementInputs()
        {
            var res = _movementService.GetAmountOfPendingMovementInputs();
            return Ok(new { value = res.ToString() });
        }

        [HttpPost("getAmountOfPendingMovementOutputs")]
        public ActionResult GetAmountOfPendingMovementOutputs()
        {
            var res = _movementService.GetAmountOfPendingMovementOutputs();
            return Ok(new { value = res.ToString() });
        }

        [HttpPost("cancelMovement")]
        public async Task<IActionResult> CancelMovement([FromBody] IdRequest idRequest)
        {
            var res = await _movementService.CancelMovement(idRequest.id);
            return Ok(res);
        }
        #endregion

    }
}
