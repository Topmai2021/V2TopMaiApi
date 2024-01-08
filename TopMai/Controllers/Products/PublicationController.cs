using Common.Utils.Helpers;
using Infraestructure.Entity.DTOs;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.DTO.Products;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Handlers;

namespace TopMai.Controllers.Products
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class PublicationController : ControllerBase
    {
        #region Attributes
        private readonly IPublicationService _publicationService;
        #endregion

        #region Builder
        public PublicationController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }
        #endregion

        #region Services
        // GET: api/<ValuesController>
        [HttpGet("getAll")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] int page = 1, int limit=100000000)
        {
            var Publication = await _publicationService.GetAll(page, limit);
            return Ok(Publication);
        }

        [HttpPost("get")]
        [AllowAnonymous]

        public async Task<IActionResult> Get([FromBody] IdRequest idRequest)
        {
            return Ok(await _publicationService.Get(idRequest.id));
        }

        [HttpPost("getActivePublications")]
        [AllowAnonymous]

        public ActionResult GetActivePublications(int? page)
        {
            return Ok(_publicationService.GetActivePublications(page));
        }

        [HttpPost("create")]
        public ActionResult Post([FromBody] PublicationDTO publication)
        {
            if (!Helper.IsYourUser(Request.Headers["Authorization"], (Guid)publication.PublisherId))
                return BadRequest(new { error = "Las credenciales no son válidas" });

            return Ok(_publicationService.Post(publication));
        }

        [HttpPost("edit")]
        public ActionResult Put([FromBody] PublicationUpdateRequest publication)
        {
            var res = _publicationService.Put(publication);
            if (res.GetType() == typeof(Publication))
                return Ok(new { value = publication.Id.ToString() });

            return BadRequest(res);
        }

        /*  [HttpDelete("delete")]
          [AllowAnonymous]
          public ActionResult Delete([FromBody] IdRequest idRequest)
          {
              var res = _publicationService.Delete(idRequest.id);
              if (res.GetType() == typeof(Publication))
                  return Ok();

              return BadRequest(res);
          }*/

        [HttpPost("searchPublication")]
        [AllowAnonymous]

        public ActionResult SearchPublication([FromBody] SearchRequest searchRequest)
        {
            var res = _publicationService.SearchPublication(searchRequest.query, searchRequest.filter);
            if (res.GetType() == typeof(List<Publication>))
                return Ok(res);

            return BadRequest(res);
        }


        [HttpGet("getHomePublications")]
        [AllowAnonymous]

        public IActionResult GetHomePublications()
        {
            List<ConsultPublication_Dto> result = _publicationService.GetHomePublications();
            return Ok(result);
        }

        [HttpPost("getPublicationsBySubcategory")]
        [AllowAnonymous]

        public ActionResult GetPublicationsBySubcategory([FromBody] IdRequest idRequest)
        {
            var res = _publicationService.GetPublicationsBySubcategory(idRequest.id);
            if (res.GetType() == typeof(List<Publication>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpGet("getPublicationsByCategory")]
        [AllowAnonymous]

        public ActionResult GetPublicationsByCategory([FromQuery] MessageRequest publicationRequest)
        {
            var res = _publicationService.GetPublicationsByCategory(publicationRequest);

            var response = new ResponseDto()
            {
                IsSuccess = res != null,
                Error = res == null ? "No se encontraron resultados de busqueda" : string.Empty,
                Message = res == null ? "No se encontraron resultados de busqueda" : string.Empty,
                Result = res
            };

            return Ok(response);
        }


        [HttpPost("removeMultiplePublications")]

        public ActionResult RemoveMultiplePublications([FromBody] List<IdRequest> idRequests)
        {
            List<Guid> ids = new List<Guid>();
            foreach (var idRequest in idRequests)
            {
                ids.Add(idRequest.id);
            }
            var res = _publicationService.RemoveMultiplePublications(ids);
            if (res.GetType() == typeof(List<Publication>))
            {
                List<Publication> publications = (List<Publication>)res;
                //foreach (var publication in publications)
                //{
                //    if (!Security.IsUser(Request.Headers["Authorization"], (Guid)publication.PublisherId) && !Security.IsAdmin(Request.Headers["Authorization"]))
                //    {
                //        return BadRequest(new { error = "
                //        no son válidas" });
                //    }

                //}
                return Ok(res);
            }
            return BadRequest(res);

        }

        [HttpPost("removeImagePublications")]

        public ActionResult RemoveImagePublications([FromBody] IdRequest idRequest)
        {
            try
            {
                var token = Request.Headers["Authorization"];

                //Publication publication = _publicationService.Get(idRequest.id);
                var res = _publicationService.RemoveImagePublications(idRequest.id);
                if (res.GetType() == typeof(bool)) return Ok(res);
                return BadRequest(res);


            }
            catch (Exception e)
            {

                return BadRequest(e);

            }
        }

        [HttpPost("renewMultiplePublications")]
        public ActionResult RenewMultiplePublications([FromBody] List<IdRequest> idRequests)
        {
            List<Guid> ids = new List<Guid>();
            foreach (var idRequest in idRequests)
            {
                ids.Add(idRequest.id);
            }
            var res = _publicationService.RenewMultiplePublications(ids);
            if (res.GetType() == typeof(List<Publication>))
                return Ok(res);

            return BadRequest(res);
        }


        [HttpPost("getPublicationsByProfile")]
        [AllowAnonymous]

        public ActionResult GetPublicationsByProfile([FromBody] IdRequest idRequest)
        {
            var res = _publicationService.GetPublicationsByProfile(idRequest.id);
            if (res.GetType() == typeof(List<Publication>))
                return Ok(res);

            if (res == null)
                return BadRequest(res);

            return Ok(res);
        }

        [HttpPost("getImagesByPublication")]
        [AllowAnonymous]

        public ActionResult GetImagesByPublication([FromBody] IdRequest idRequest)
        {
            var res = _publicationService.GetImagesByPublication(idRequest.id);
            return Ok(res);
        }

        [HttpPost("addImageToPublication")]
        public ActionResult AddImageToPublication([FromBody] PublicationImageRequest publicationImageRequest)
        {
            var res = _publicationService.AddImageToPublication(publicationImageRequest.idImage, publicationImageRequest.idPublication, publicationImageRequest.number);
            var response = new ResponseDto()
            {
                IsSuccess = res != null,
                Message = res == null ? "imagen no publicada" : "imagen publicada",
                Result = res
            };

            return Ok(response);
        }

        [HttpPost("addShippmentTypeToPublication")]
        public ActionResult AddShippmentTypeToPublication([FromBody] PublicationShippmentRequest publicationShippmentRequest)
        {
            var res = _publicationService.AddShippmentTypeToPublication(publicationShippmentRequest.idShippmentType, publicationShippmentRequest.idPublication, publicationShippmentRequest.price);
            if (res.GetType() == typeof(PublicationShippmentType))
            {
                PublicationShippmentType publicationShippmentType = (PublicationShippmentType)res;
                return Ok(new { value = publicationShippmentType.Id });
            }
            return BadRequest(res);
        }

        [HttpPost("getShippmentTypeByPublication")]
        [AllowAnonymous]

        public ActionResult GetShippmentTypeByPublication([FromBody] IdRequest idRequest)
        {
            var res = _publicationService.GetShippmentTypeByPublication(idRequest.id);
            if (res.GetType() == typeof(List<PublicationShippmentType>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("addNewVisit")]
        [AllowAnonymous]

        public async Task<IActionResult> AddNewVisit([FromBody] IdRequest idRequest)
        {
            var res = await _publicationService.AddNewVisitEndPoint(idRequest.id);
            return Ok(new { value = res });
        }

        [HttpPost("delete")]
        [AllowAnonymous]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var publication = _publicationService.Delete(idRequest.id); // Assuming _publicationService contains the business logic

            if (publication == null)
            {
                return NotFound("Publication not found");
            }

            return Ok("Publication deleted successfully");
        }

        #endregion
    }
}
