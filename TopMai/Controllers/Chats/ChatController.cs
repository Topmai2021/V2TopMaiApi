using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Chats;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Domain.Services.Users.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Chats
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ChatController : ControllerBase
    {
        #region Attributes
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        #endregion

        #region Builder
        public ChatController(IChatService chatService, IUserService userService)
        {
            _chatService = chatService;
            _userService = userService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(_chatService.GetAll());

        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_chatService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] Chat chat)
        {
            var res = await _chatService.Post(chat);
            return Ok(new { value = res.Id.ToString() });
        }

        [HttpPost("hasMoreMessages")]
        public ActionResult HasMoreMessages([FromBody] MessageRequest messageRequest)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);

            if (string.IsNullOrEmpty(idUser))
                return BadRequest("Las credenciales no son válidas");

            return Ok(_chatService.HasMoreMessages(messageRequest.id, Guid.Parse(idUser), messageRequest.page, 5));

        }

        [HttpPost("getLastMessage")]
        public ActionResult GetLastMessage([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);

            var res = _chatService.GetLastMessage(idRequest.id, Guid.Parse(idUser));
            return Ok(res);
        }

        [HttpPost("verifyChat")]
        public ActionResult VerifyChat([FromBody] VerifyChatDto chat)
        {
            var result = _chatService.VerifyChat(chat);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result != null,
                Message = result == null ? "No existe Chat" : "Si existe chat",
                Result = result
            };

            return Ok(response);
        }

        [HttpPost("getChatId")]
        public ActionResult GetChatId([FromBody] Chat chat)
        {
            var res = _chatService.GetChatId((Guid)chat.IdProfileSender, (Guid)chat.IdProfileReceiver, null);
            return Ok(new { value = res.Id.ToString() });
        }

        [HttpGet("getUserChats")]
        public async Task<IActionResult> GetUserChats(Guid idRequest, int page)
        {
            List<ChatDto> res = await _chatService.GetUserChats(idRequest, page);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = res
            };

            return Ok(response);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Delete([FromBody] IdRequest idRequest)
        {
            var res = await _chatService.Delete(idRequest.id);
            //return Ok(res);
            return Ok(new { data = res });
        }

        [HttpPost("connectToOneSignal")]
        public async Task<IActionResult> ConnectToOneSignal([FromBody] ConnectionRequest connectionRequest)
        {
            var res = await _chatService.ConnectToOneSignal((Guid)connectionRequest.id, connectionRequest.connectionId);
            //return Ok(res);
            return Ok(new { data = res });
        }

        [HttpPost("disconnectOneSignal")]
        public async Task<IActionResult> DisconnectOneSignal([FromBody] IdRequest idRequest)
        {
            var res = await _chatService.DisconnectOneSignal(idRequest.id);
            // return Ok(res);
            return Ok(new { data = res });
        }
        #endregion

    }
}
