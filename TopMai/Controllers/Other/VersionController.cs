using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopMai.Handlers;
using TopMai.Domain.Services.Other.Interfaces;
using Infraestructure.Entity.Request;
using Infraestructure.Entity.Entities.Other;
using Version = Infraestructure.Entity.Entities.Other.Version;
using static Common.Utils.Constant.Const;
using Microsoft.AspNetCore.Authorization;

namespace TopMai.Controllers.Version
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class VersionController : ControllerBase
    {
        #region Attributes
        private IVersionService VersionService;
        #endregion


        #region Builder
        public VersionController(IVersionService versionService)
        {
            this.VersionService = versionService;

        }
        #endregion


        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]

        public ActionResult Get()
        {

            return Ok(this.VersionService.GetAll());



        }

        [HttpPost("getActualVersionAndroid")]
        [AllowAnonymous]

        public ActionResult GetActualVersionAndroid()
        {

            return Ok(this.VersionService.GetActualVersion("android", false));

        }


        [HttpPost("getActualVersionIos")]
        [AllowAnonymous]

        public ActionResult GetActualVersionIos()
        {

            return Ok(this.VersionService.GetActualVersion("ios", false));

        }

        [HttpPost("getLastRequiredVersionAndroid")]
        [AllowAnonymous]

        public ActionResult GetLastRequiredVersionAndroid()
        {

            return Ok(this.VersionService.GetActualVersion("android", true));

        }

        [HttpPost("getLastRequiredVersionIos")]
        [AllowAnonymous]

        public ActionResult GetLastRequiredVersionIos()
        {

            return Ok(this.VersionService.GetActualVersion("ios", true));

        }




        [HttpPost("get")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            var token = Request.Headers["Authorization"];


            return Ok(VersionService.Get(idRequest.id));

        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Post([FromBody] Infraestructure.Entity.Entities.Other.Version version)
        {
            try
            {

                var res = VersionService.Post(version);
                if (res.GetType() == typeof(Infraestructure.Entity.Entities.Other.Version)) return Ok(new { id = version.Id.ToString() });
                return BadRequest(res);


            }
            catch (Exception e)
            {

                return BadRequest(e);

            }

        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Put([FromBody] Infraestructure.Entity.Entities.Other.Version version)
        {
            try
            {


                var res = VersionService.Put(version);
                if (res.GetType() == typeof(Infraestructure.Entity.Entities.Other.Version)) return Ok(new { value = version.Id.ToString() });
                return BadRequest(res);



            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            try
            {


                var res = VersionService.Delete(idRequest.id);
                if (res.GetType() == typeof(Infraestructure.Entity.Entities.Other.Version)) return Ok();
                return BadRequest(res);


            }
            catch (Exception e)
            {

                return BadRequest(e);

            }
        }

    }
    #endregion
    }
