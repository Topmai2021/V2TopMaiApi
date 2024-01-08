using Common.Utils.Resources;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Chats;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Chats
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ChatConfigurationController : ControllerBase
    {
        #region Attributes
        private readonly IChatConfigurationService _chatConfigurationService;
        #endregion

        #region Builder
        public ChatConfigurationController(IChatConfigurationService chatConfigurationService)
        {
            _chatConfigurationService = chatConfigurationService;
        }
        #endregion

        #region Services

        [HttpPost("get")]
        public ActionResult Get([FromBody] ChatConfiguration chatConfiguration)
        {
            return Ok(_chatConfigurationService.Get(chatConfiguration));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Put(ChatConfigurationDto chatConfiguration)
        {
            IActionResult action;
            bool result = await _chatConfigurationService.Put(chatConfiguration);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemUpdated : GeneralMessages.ItemNoUpdated,
                Error = result ? GeneralMessages.ItemUpdated : GeneralMessages.ItemNoUpdated,
                Result = result,
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return Ok(action);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _chatConfigurationService.Delete(idRequest.id);
            if (res.GetType() == typeof(ChatConfiguration)) return Ok();

            return BadRequest(res);
        }

        [HttpPost("clearChat")]
        public ActionResult ClearChat([FromBody] ChatConfiguration chatConfiguration)
        {
            var res = _chatConfigurationService.ClearChat(chatConfiguration);
            if (res.GetType() == typeof(ChatConfiguration))
                return Ok();

            return BadRequest(res);
        }
        #endregion
    }
}
