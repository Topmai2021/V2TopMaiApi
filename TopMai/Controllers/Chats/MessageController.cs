using Common.Utils.Helpers;
using Common.Utils.Resources;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Chats;
using TopMai.Domain.Services.Chats;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Chats
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class MessageController : ControllerBase
    {
        #region Attributes
        private readonly IMessageService _messageService;
        #endregion

        #region Builder
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        #endregion

        #region Services

        [HttpGet("GetAllMessageByChat")]
        public IActionResult GetAllMessageByChat(Guid chatId, int page)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);

            var result = _messageService.GetAllByChat(chatId, Guid.Parse(idUser), page);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            };

            return Ok(response);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Post(AddMessageDto message)
        {
            IActionResult action;
            bool result = await _messageService.Post(message);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = string.Empty,
                Result = result,
                Error = string.Empty
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpPost("createSupportMessage")]
        public async Task<IActionResult> CreateSupportMessage([FromBody] SupportMessageRequest supportMessageRequest)
        {
            var res = await _messageService.CreateSupportMessage(supportMessageRequest.UserId, supportMessageRequest.Content);
            return Ok(res);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete( Guid messageId)
        {
            IActionResult action;
            bool result = await _messageService.Delete(messageId);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted,
                Error = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted,
                Result = result,
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return Ok(action);
        }
        #endregion
    }
}
