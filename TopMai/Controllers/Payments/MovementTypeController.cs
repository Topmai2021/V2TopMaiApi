using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Payments
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class MovementTypeController : ControllerBase
    {
        #region Attributes
        private readonly IMovementTypeService _movementTypeService;
        #endregion

        #region Builder
        public MovementTypeController(IMovementTypeService movementTypeService)
        {
            _movementTypeService = movementTypeService;
        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        [AllowAnonymous]
        public ActionResult Get()
        {
            return Ok(this._movementTypeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] int idRequest)
        {
            var token = Request.Headers["Authorization"];

            if (Helper.IsAdmin(token))
                return Ok(this._movementTypeService.GetAll());

            return Ok(_movementTypeService.Get(idRequest));

        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Post([FromBody] MovementType movementType)
        {
            var res = await _movementTypeService.Post(movementType);
            return Ok(new { value = movementType.Id.ToString() });
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Put([FromBody] MovementType movementType)
        {
            var res = await _movementTypeService.Put(movementType);
            return Ok(new { value = movementType.Id.ToString() });
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Delete([FromBody] int idRequest)
        {
            var res = await _movementTypeService.Delete(idRequest);
            return Ok(res);
        }

        #endregion
    }
}
