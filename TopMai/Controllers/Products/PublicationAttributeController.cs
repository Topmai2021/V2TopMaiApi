using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;
using PublicationAttribute = Infraestructure.Entity.Entities.Products.PublicationAttribute;

namespace TopMai.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class PublicationAttributeController : ControllerBase
    {
        #region Attributes
        private IPublicationAttributeService PublicationAttributeService;
        #endregion


        #region Builder
        public PublicationAttributeController(IPublicationAttributeService PublicationAttributeService)
        {
            this.PublicationAttributeService = PublicationAttributeService;

        }
        #endregion


        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]

        public ActionResult Get()
        {

            return Ok(this.PublicationAttributeService.GetAll());



        }




        [HttpPost("get")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Get([FromBody] IdRequest idRequest)
        {


            return Ok(PublicationAttributeService.Get(idRequest.id));

        }

        [HttpPost("create")]

        public ActionResult Post([FromBody] PublicationAttribute publicationAttribute)
        {


            try
            {


                var res = PublicationAttributeService.Post(publicationAttribute);
                if (res.Result.GetType() == typeof(PublicationAttribute))
                {
                    PublicationAttribute publicationAttributeRes = (PublicationAttribute)res.Result;
                    return Ok(new { id = publicationAttributeRes.Id.ToString() });
                }
                return BadRequest(res.Result);


            }
            catch (Exception e)
            {

                return BadRequest(e);

            }


        }

        [HttpPost("edit")]

        public ActionResult Put([FromBody] PublicationAttribute PublicationAttribute)
        {
            try
            {


                var res = PublicationAttributeService.Put(PublicationAttribute);
                if (res.GetType() == typeof(PublicationAttribute)) return Ok(new { value = PublicationAttribute.Id.ToString() });
                return BadRequest(res);



            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpPost("delete")]

        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            try
            {


                var res = PublicationAttributeService.Delete(idRequest.id);
                if (res.GetType() == typeof(PublicationAttribute)) return Ok();
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
