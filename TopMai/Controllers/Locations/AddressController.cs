using Infraestructure.Entity.Entities.Locations;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Locations.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Locations
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class AddressController : ControllerBase
    {
        #region Attributes
        private readonly IAddressService _addressService;
        #endregion

        #region builder
        public AddressController(IAddressService addressService)
        {
            this._addressService = addressService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(_addressService.GetAll());
        }

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_addressService.Get(idRequest.id));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] Address address)
        {
            var res = _addressService.Post(address);
            if (res.GetType() == typeof(Address))
                return Ok(new { id = address.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] Address address)
        {
            var res = _addressService.Put(address);
            if (res.GetType() == typeof(Address))
                return Ok(new { value = address.Id.ToString() });

            return BadRequest(res);

        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _addressService.Delete(idRequest.id);
            if (res.GetType() == typeof(Address))
                return Ok();

            return BadRequest(res);
        }

        [HttpPost("getAddressesByProfileId")]
        public ActionResult GetAddressesByProfileId([FromBody] IdRequest idRequest)
        {
            var res = _addressService.GetAddressesByProfileId(idRequest.id);
            if (res.GetType() == typeof(List<Address>))
                return Ok(res);

            return BadRequest(res);
        }

        #endregion
    }
}
