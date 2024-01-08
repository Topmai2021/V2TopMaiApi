using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO.SubCategory;
using TopMai.Domain.Services.Products;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Products
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class SubcategoryController : ControllerBase
    {
        #region Attributes
        private readonly ISubcategoryService _subcategoryService;
        #endregion

        #region Builder
        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        [AllowAnonymous]

        public ActionResult Get(int page = 1, int pageSize = 1000000)
        {
            var response = _subcategoryService.GetAll(page, pageSize);
            return Ok(response);
        }
        //public ActionResult Get()
        //{
        //    return Ok(_subcategoryService.GetAll());
        //}

        [HttpPost("get")]
        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_subcategoryService.Get(idRequest.id));
        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Post([FromBody] SubCategoryDTO subcategory)
        {
            var res = _subcategoryService.Post(subcategory);
            if (res.GetType() == typeof(Subcategory))
                return Ok(new { id = subcategory.Id.ToString() });

            return BadRequest(res);
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] SubCategoryDTO subCategory)
        {
            var result = _subcategoryService.Put(subCategory);

            if (result is Subcategory)
            {
                // If the result is a Subcategory, return it as Ok (200)
                return Ok(result);
            }
            else if (result is string && result.ToString() != "")
            {
                // If there's an error message in the result, return a BadRequest (400) with the error message
                return BadRequest(new { error = result.ToString() });
            }
            else
            {
                // If the result is neither Subcategory nor an error message, return 500 Internal Server Error
                return StatusCode(500);
            }
            /*var res = _subcategoryService.Put(subCategory);
            if (res.GetType() == typeof(Subcategory))
                return Ok(new { value = subCategory.Id.ToString() });

            return BadRequest(res);*/
        }

        [HttpPost("getSubcategoriesByCategoryId")]
        [AllowAnonymous]

        public ActionResult GetSubcategoriesByCategory([FromBody] IdRequest idRequest)
        {
            var res = _subcategoryService.GetSubcategoriesByCategory(idRequest.id);
            if (res.GetType() == typeof(List<Subcategory>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("getSubcategoriesBySubcategoryId")]
        [AllowAnonymous]

        public ActionResult GetSubcategoriesBySubcategoryId([FromBody] IdRequest idRequest)
        {
            var res = _subcategoryService.GetSubcategoriesBySubcategoryId(idRequest.id);
            if (res.GetType() == typeof(List<Subcategory>))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("getSubcategoriesMostUsed")]
        [AllowAnonymous]
        public ActionResult getSubcategoriesMostUsed()
        {
            var res = _subcategoryService.getSubcategoriesMostUsed();
            if (res.GetType() == typeof(Category))
                return Ok(res);

            return BadRequest(res);
        }

        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        [AllowAnonymous]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _subcategoryService.Delete(idRequest.id);

            if (res != null)
            {
                // Check the actual type of 'res' and cast it appropriately.
                if (res is Subcategory subcategory)
                {
                    return Ok(new { value = subcategory.Deleted });
                }
            }

            return NotFound("No Data Found");
            //var res = _subcategoryService.Delete(idRequest.id);
            //if (res != null)
            //    return Ok(new { value = res.Deleted });

            //return NotFound("No Data Found");
        }
        #endregion
    }
}
