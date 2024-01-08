using Common.Utils.Exceptions;
using Common.Utils.Resources;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using TopMai.Domain.DTO;

namespace TopMai.Handlers
{
    public class CustomExceptionHandler : ExceptionFilterAttribute
    {
        /// <summary>
        /// Metodo encargado de capturar todas las Excepciones del proyecto,
        /// Se debe agregar el decorador a cada controller [TypeFilter(typeof(CustomExceptionHandler))]
        /// </summary>
        /// <param name="exception"> Parametro donde queda capturada la Exception </param>
        public override void OnException(ExceptionContext context)
        {
            HttpResponseException responseExeption = new HttpResponseException();

            ResponseDto response = new ResponseDto();

            if (context.Exception is BusinessException)
            {
                responseExeption.Status = StatusCodes.Status400BadRequest;
                response.Error = context.Exception.Message;
                response.Message = context.Exception.Message;
                context.ExceptionHandled = true;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                responseExeption.Status = StatusCodes.Status401Unauthorized;
                response.Result = new { message = context.Exception.Message };
                response.Error = "Usuario no autenticado correctamente";
                response.Message = "Usuario no autenticado correctamente";
                context.ExceptionHandled = true;
                Log.Error(GeneralMessages.Error401);
            }
            else
            {
                response.Result = JsonConvert.SerializeObject(context.Exception);
                responseExeption.Status = StatusCodes.Status500InternalServerError;
                response.Error = GeneralMessages.Error500;
                response.Message = GeneralMessages.Error500;
                context.ExceptionHandled = true;
                Log.Fatal(context.Exception, GeneralMessages.Error500);
            }

            context.Result = new ObjectResult(responseExeption.Value)
            {
                StatusCode = responseExeption.Status,
                Value = response
            };

            if (responseExeption.Status == StatusCodes.Status500InternalServerError)
                context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = GeneralMessages.Error500;


        }
    }

}
