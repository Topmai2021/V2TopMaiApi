using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Request
{
    public class UpdateBankAccountRequest
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
    }
}
