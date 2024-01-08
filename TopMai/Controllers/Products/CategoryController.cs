using Common.Utils.Resources;
using Infraestructure.Entity.DTOs;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.DTO;
using TopMai.Domain.Services.Products.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.Products
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class CategoryController : ControllerBase
    {
        #region Attributes
        private readonly ICategoryService _categoryService;
        #endregion

        #region Builder
        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        #endregion

        #region Services
        [HttpPost("getAll")]
        [AllowAnonymous]

        public ActionResult Get(int page = 1, int pageSize = 1000000)
        {
            var (categories, totalCount) = _categoryService.GetAll(page, pageSize);

            var response = new
            {
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                Data = categories
            };

            return Ok(response);
        }

        /* public ActionResult Get()
         {
             return Ok(_categoryService.GetAll());

         }*/

        [HttpPost("get")]
        [AllowAnonymous]

        public ActionResult Get([FromBody] IdRequest idRequest)
        {
            return Ok(_categoryService.Get(idRequest.id));
        }

        [HttpPost("create")]
        [AllowAnonymous]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Post([FromBody] CategoryDTO category)
        {
            IActionResult actionResult;

            bool result = await _categoryService.Post(category);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Message = result ? GeneralMessages.ItemInserted : GeneralMessages.ItemNoInserted,
                Error = result ? GeneralMessages.ItemInserted : GeneralMessages.ItemNoInserted,
                Result = result
            };

            if (result)
                actionResult = Ok(response);
            else
                actionResult = BadRequest(response);

            return actionResult;
        }

        [HttpPost("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Put([FromBody] UpdateRequestCategory category)
        {
            var res = _categoryService.Put(category);
            if (res.GetType() == typeof(Category))
                return Ok(new { value = category.Id.ToString() });

            return BadRequest(res);
        }
        [HttpPost("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
         //[AllowAnonymous]
        public ActionResult Delete([FromBody] IdRequest idRequest)
        {
            var res = _categoryService.Delete(idRequest.id);
            if (res!=null)
                return Ok(new { value = res.Deleted});

            return NotFound("No Data Found");
        }

        //[HttpPost("delete")]
        //public IActionResult DeleteCategory(Guid id)
        //{
        //    var result = _categoryService.Delete(id);

        //    if (result == null)
        //    {
        //        // Handle the case where an error occurred, such as an invalid ID.
        //        return BadRequest("No Data Found");
        //    }

        //    // Check if the result has an 'error' property and it's a string.
        //    if (result.GetType().GetProperty("error") != null && result.GetType().GetProperty("error").PropertyType == typeof(string))
        //    {
        //        // It's an error object; return the error message.
        //        string errorMessage = result.GetType().GetProperty("error").GetValue(result).ToString();
        //        return BadRequest(errorMessage);
        //    }

        //    // If no error property is found, it's considered a successful response.
        //    return Ok(result);
        //}



        #endregion
    }
}
