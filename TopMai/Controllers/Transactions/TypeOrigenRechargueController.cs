using Common.Utils.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Transactions;
using TopMai.Domain.Services.Transactions.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Transactions
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class TypeOrigenRechargueController : ControllerBase
    {
        #region Attributes
        private readonly ITypeOrigenRechargueServices _typeOrigenRechargue;
        #endregion

        #region Builder
        public TypeOrigenRechargueController(ITypeOrigenRechargueServices typeOrigenRechargue)
        {
            _typeOrigenRechargue = typeOrigenRechargue;
        }
        #endregion

        #region Services
        [HttpGet("GeAlltTypeOrigenRechargue")]
        public IActionResult GeAlltTypeOrigenRechargue()
        {
            var result = _typeOrigenRechargue.GeAlltTypeOrigenRechargue();
            return Ok(new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            });
        }


        [HttpPost("UpdateTypeOrigenRechargue")]
        public async Task<IActionResult> UpdateTypeOrigenRechargue(TypeOrigenRechargue_Dto origen)
        {
            IActionResult action;
            bool result = await _typeOrigenRechargue.UpdateTypeOrigenRechargue(origen);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemInserted : GeneralMessages.ItemNoInserted,
                Result = result
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }
        #endregion
    }
}
