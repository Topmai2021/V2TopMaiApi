using Infraestructure.Entity.Entities.Users;
using Infraestructure.Entity.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopMai.Domain.Services.Users.Interfaces;
using TopMai.Handlers;
using static Common.Utils.Constant.Const;

namespace TopMai.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class RoleController : ControllerBase
    {
        #region Attributes
        private IRoleService _roleService;
        #endregion

        #region Builder
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        #endregion

        #region Services
        [HttpGet("getAll")]
        [CustomRolFilterImplement(Roles.Admin)]
        public ActionResult Get()
        {
            return Ok(_roleService.GetAll());
        }

        [HttpGet("get")]
        public ActionResult Get([FromBody] int id)
        {
            return Ok(_roleService.Get(id));
        }

        [HttpPost("create")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Post([FromBody] Role role)
        {
            var res = await _roleService.Post(role);
            return Ok(new
            {
                id = res
            });
        }

        [HttpPut("edit")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Put([FromBody] Role role)
        {
            var id = await _roleService.Put(role);
            return Ok(new
            {
                value = id
            });
        }

        [HttpDelete("delete")]
        [CustomRolFilterImplement(Roles.Admin)]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var res = await _roleService.Delete(id);
            return Ok(res);
        }
        #endregion
    }
}
