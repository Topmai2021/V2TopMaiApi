using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Chats
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class MessageTypeController : ControllerBase
    {
        #region Attributes
        private readonly IMessageTypeService _messageTypeService;
        #endregion

        #region Builder
        public MessageTypeController(IMessageTypeService messageTypeService)
        {
            _messageTypeService = messageTypeService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(_messageTypeService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] int id)
        {
            return Ok(_messageTypeService.Get(id));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] MessageType messageType)
        {
            var res = await _messageTypeService.Post(messageType);
            return Ok(new { id = messageType.Id.ToString() });
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Put([FromBody] MessageType messageType)
        {
            var res = await _messageTypeService.Put(messageType);
            return Ok(new { id = messageType.Id.ToString() });
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var res = await _messageTypeService.Delete(id);
            return Ok(res);
        }
        #endregion

    }
}
