using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Request
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public bool Validated { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
    }
}
