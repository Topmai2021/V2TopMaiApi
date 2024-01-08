using Common.Utils.Helpers;
using Common.Utils.Resources;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.Services.Profiles.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Profiles
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class ContactController : ControllerBase
    {
        #region Attributes
        private readonly IContactService _contactService;
        #endregion

        #region Builder
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        #endregion

        #region Services

        [HttpGet("get")]
        public ActionResult Get(Guid idContact)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            var contact = _contactService.Get(idContact, Guid.Parse(idUser));
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = contact
            };

            return Ok(response);
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Contact contact)
        {
            var res = _contactService.Post(contact);
            if (res.GetType() == typeof(Contact))
                return Ok(new { value = contact.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Contact contact)
        {
            var res = _contactService.Put(contact);
            if (res.GetType() == typeof(Contact))
                return Ok(new { value = contact.Id.ToString() });

            return BadRequest(res);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid idContact)
        {
            IActionResult action;

            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _contactService.Delete(idContact, Guid.Parse(idUser));
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted,
                Error = result ? string.Empty : GeneralMessages.ItemNoDeleted,
                Result = result
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpPost("addContactById")]
        public ActionResult AddContactById([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);

            var res = _contactService.AddContactById(Guid.Parse(idUser), idRequest.id);
            if (res.GetType() == typeof(Contact))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("addMultipleContactsByPhones")]
        public ActionResult AddMultipleContactsByPhones([FromQuery] IdRequest idRequest, [FromBody] List<PhoneRequest> phone)
        {
            List<ContactToInvite> phoneContacts = new List<ContactToInvite>();
            foreach (PhoneRequest phoneRequest in phone)
            {
                ContactToInvite contactToInvite = new ContactToInvite();
                contactToInvite.Phone = phoneRequest.phone;
                contactToInvite.FullName = phoneRequest.fullName;
                phoneContacts.Add(contactToInvite);
            }
            var res = _contactService.AddMultipleContactsByPhones(idRequest.id, phoneContacts);
            if (res.GetType() == typeof(List<ContactToInvite>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpGet("GetMyContacts")]
        public IActionResult GetMyContacts()
        {
            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);

            List<ContactDto> result = _contactService.GetMyContacts(Guid.Parse(idUser));
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Message = string.Empty,
                Result = result
            };

            return Ok(response);
        }

        [HttpPost("BlockContact")]
        public async Task<IActionResult> BlockContact(Guid idContact)
        {
            IActionResult action;

            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _contactService.BlockContact(idContact, Guid.Parse(idUser));
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemProcess : GeneralMessages.ItemNotProcess,
                Error = result ? string.Empty : GeneralMessages.ItemNotProcess,
                Result = result
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpPost("UnblockContact")]
        public async Task<IActionResult> UnblockContact(Guid idContact)
        {
            IActionResult action;

            var token = Request.Headers["Authorization"];
            string idUser = Helper.GetClaimValue(token, TypeClaims.IdUser);
            bool result = await _contactService.UnblockContact(idContact, Guid.Parse(idUser));
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemProcess : GeneralMessages.ItemNotProcess,
                Error = result ? string.Empty : GeneralMessages.ItemNotProcess,
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
