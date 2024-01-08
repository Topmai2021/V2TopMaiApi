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
    public class QuestionAnswerController : ControllerBase
    {
        #region Attributes
        private readonly IQuestionAnswerService _questionAnswerService;
        #endregion

        #region Buil.der
        public QuestionAnswerController(IQuestionAnswerService questionAnswerService)
        {
            _questionAnswerService = questionAnswerService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        public ActionResult Get()
        {
            return Ok(this._questionAnswerService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_questionAnswerService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] QuestionAnswer questionAnswer)
        {
            var res = _questionAnswerService.Post(questionAnswer);
            if (res.Result.GetType() == typeof(QuestionAnswer))
                return Ok(new { id = questionAnswer.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] QuestionAnswer questionAnswer)
        {
            var res = _questionAnswerService.Put(questionAnswer);
            if (res.Result.GetType() == typeof(QuestionAnswer))
                return Ok(new { value = questionAnswer.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("validateAnswer")]
        public ActionResult ValidateAnswer([FromBody] QuestionAnswer questionAnswer)
        {
            var res = _questionAnswerService.ValidateAnswer((Guid)questionAnswer.ProfileId, questionAnswer.Answer,(Guid)questionAnswer.QuestionId);
            if (res.Result.GetType() == typeof(bool))
                return Ok(new { value = res.Result });

            return BadRequest(res);
        }


        [HttpPost("getQuestionsByProfileId")]
        
        public ActionResult GetQuestionsByProfileId([FromBody] IdRequest idRequest)
        {
            return Ok(_questionAnswerService.GetQuestionsByProfileId(idRequest.id));
        }


        [HttpPost("delete")]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _questionAnswerService.Delete(idRequest.id);
            if (res.Result.GetType() == typeof(QuestionAnswer))
                return Ok();

            return BadRequest(res);
        }

        #endregion
    }
}
