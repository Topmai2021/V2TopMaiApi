using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Response.ChatResponses
{
    public class ChatVerifyResponse
    {
        public Guid Id { get; set; }
        public bool Verified { get; set; }
    }
}
