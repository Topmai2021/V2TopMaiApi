using Infraestructure.Entity.Entities.Chats;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Chats.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Chats
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class RoomOfConversationController : ControllerBase
    {
        #region Attributes
        private IRoomOfConversationService _roomOfConversationService;
        #endregion

        #region Builder
        public RoomOfConversationController(IRoomOfConversationService roomOfConversationService)
        {
            _roomOfConversationService = roomOfConversationService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._roomOfConversationService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_roomOfConversationService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] RoomOfConversation roomOfConversation)
        {
            var res = _roomOfConversationService.Post(roomOfConversation);
            if (res.GetType() == typeof(RoomOfConversation))
                return Ok(roomOfConversation);

            return BadRequest(res);
        }

        [HttpPost("getRoomsByUserId")]
        public ActionResult GetRoomsByUserId([FromBody] IdRequest idRequest)
        {
            var res = _roomOfConversationService.GetRoomsByUserId(idRequest.id);
            if (res.GetType() == typeof(List<RoomOfConversation>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] RoomOfConversation roomOfConversation)
        {
            var res = _roomOfConversationService.Put(roomOfConversation);
            if (res.GetType() == typeof(RoomOfConversation))
                return Ok(new { value = roomOfConversation.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _roomOfConversationService.Delete(idRequest.id);
            if (res.GetType() == typeof(RoomOfConversation))
                return Ok();

            return BadRequest(res);
        }
        #endregion
    }
}
