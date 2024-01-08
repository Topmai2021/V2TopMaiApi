using Common.Utils.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using TopMai.Domain.DTO;

namespace TopMai.Handlers
{
    public class CustomValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                HttpResponseException responseException = new HttpResponseException()
                {
                    Status = StatusCodes.Status400BadRequest
                };

                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in context.ModelState)
                {
                    string errorMessage = $" - {item.Value.Errors.Select(x => x.ErrorMessage).FirstOrDefault()}";
                    stringBuilder.AppendLine(errorMessage);
                }

                ResponseDto response = new ResponseDto()
                {
                    Message = stringBuilder.ToString(),
                    Result = new UnprocessableEntityObjectResult(context.ModelState).Value!
                };

                context.Result = new ObjectResult(responseException)
                {
                    StatusCode = responseException.Status,
                    Value = response
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }

}
