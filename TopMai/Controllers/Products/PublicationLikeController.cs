using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopMai.Handlers;
using TopMai.Domain.Services.Other.Interfaces;
using Infraestructure.Entity.Request;
using Infraestructure.Entity.Entities.Other;
using PublicationAttribute = Infraestructure.Entity.Entities.Products.PublicationAttribute;
using static Common.Utils.Constant.Const;
using Microsoft.AspNetCore.Authorization;
using TopMai.Domain.Services.Products.Interfaces;
using Infraestructure.Entity.Entities.Products;

namespace TopMai.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]   
    [Authorize]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class PublicationLikeController : ControllerBase
    {
        #region Attributes
        private IPublicationLikeService _publicationLikeService;
        #endregion


        #region Builder
        public PublicationLikeController(IPublicationLikeService publicationLikeService)
        {
            _publicationLikeService = publicationLikeService;

        }
        #endregion


        #region Services
        // GET: api/<ValuesController>
        [HttpPost("getAll")]

        public ActionResult Get()
        {

            return Ok(_publicationLikeService.GetAll());



        }

      


        [HttpPost("get")]
        [CustomRolFilterImplement(Roles.Admin)]

        public ActionResult Get([FromBody] IdRequest idRequest)
        {


            return Ok(_publicationLikeService.Get(idRequest.id));

        }

        [HttpPost("create")]

        public ActionResult Post([FromBody] PublicationLike publicationLike)
        {
            try
            {

                var res = _publicationLikeService.Post(publicationLike);
                if (res.Result.GetType() == typeof(PublicationLike)) return Ok(new { id = publicationLike.Id.ToString() });
                return BadRequest(res);


            }
            catch (Exception e)
            {

                return BadRequest(e);

            }

        }

        [HttpPost("edit")]

        public ActionResult Put([FromBody] PublicationLike publicationLike)
        {
            try
            {


                var res = _publicationLikeService.Put(publicationLike);
                if (res.GetType() == typeof(PublicationLike)) return Ok(new { value = publicationLike.Id.ToString() });
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


                var res = _publicationLikeService.Delete(idRequest.id);
                if (res.GetType() == typeof(PublicationLike)) return Ok();
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
