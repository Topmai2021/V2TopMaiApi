using Infraestructure.Entity.Entities.Chats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Chats
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ChatTypeController : ControllerBase
    {
        #region Attributes
        private readonly IChatTypeService _chatTypeService;
        #endregion

        #region Builder
        public ChatTypeController(IChatTypeService chatTypeService)
        {
            _chatTypeService = chatTypeService;
        }
        #endregion

        #region Services

        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(_chatTypeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] int id)
        {
            return Ok(_chatTypeService.Get(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] ChatType chatType)
        {
            var res = await _chatTypeService.Post(chatType);
            return Ok(new { id = chatType.Id.ToString() });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Put([FromBody] ChatType chatType)
        {
            var res = await _chatTypeService.Put(chatType);
            return Ok(new { value = chatType.Id.ToString() });
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var res = await _chatTypeService.Delete(id);
            return Ok();
        }
        #endregion

    }
}
