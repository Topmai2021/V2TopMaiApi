using Common.Utils.Helpers;
using Infraestructure.Entity.Entities.Complaints;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Complaints.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Complaints
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ComplaintController : ControllerBase
    {
        #region Attributes
        private readonly IComplaintService _complaintService;
        #endregion

        #region Builder
        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(this._complaintService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];
            if (Helper.IsAdmin(token))
            {
                return Ok(this._complaintService.GetAll());

            }

            return Ok(_complaintService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Complaint complaint)
        {
            var res = _complaintService.Post(complaint);
            if (res.GetType() == typeof(Complaint))
            {
                Complaint complaintRes = (Complaint)res;
                return Ok(new { id = complaintRes.Id.ToString() });
            }

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] Complaint complaint)
        {
            var res = _complaintService.Put(complaint);
            if (res.GetType() == typeof(Complaint))
                return Ok(new { value = complaint.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _complaintService.Delete(idRequest.id);
            if (res.GetType() == typeof(Complaint))
                return Ok();

            return BadRequest(res);
        }
        #endregion

    }
}
