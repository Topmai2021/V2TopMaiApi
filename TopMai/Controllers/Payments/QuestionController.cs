using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Payments
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class QuestionController : ControllerBase
    {
        #region Attributes
        private readonly IQuestionService _questionService;
        #endregion

        #region Buil.der
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._questionService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_questionService.Get(idRequest.id));
        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Post([FromBody] Question question)
        {
            var res = _questionService.Post(question);
            if (res.Result.GetType() == typeof(Question))
                return Ok(new { id = question.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] Question question)
        {
            var res = _questionService.Put(question);
            if (res.Result.GetType() == typeof(Question))
                return Ok(new { value = question.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _questionService.Delete(idRequest.id);
            if (res.Result.GetType() == typeof(Question))
                return Ok();

            return BadRequest(res);
        }

        #endregion
    }
}
