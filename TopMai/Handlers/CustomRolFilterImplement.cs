using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static Common.Utils.Constant.Const;

namespace TopMai.Handlers
{
    public class CustomRolFilterImplement : TypeFilterAttribute
    {
        public CustomRolFilterImplement(string roles) : base(typeof(CustomPermissionFilterImplement))
        {
            Arguments = new object[] { roles };
        }

        private class CustomPermissionFilterImplement : IActionFilter
        {
            private readonly string _roles;

            public CustomPermissionFilterImplement(string roles)
            {
                _roles = roles;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {

            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                string token = context.HttpContext.Request.Headers["Authorization"];
                string rol = Helper.GetClaimValue(token, TypeClaims.Rol);

                bool result = (rol == _roles) || (rol == Roles.Admin);
                if (!result)
                    throw new BusinessException("Necesitas permisos de administrador");
            }
        }
    }
}
