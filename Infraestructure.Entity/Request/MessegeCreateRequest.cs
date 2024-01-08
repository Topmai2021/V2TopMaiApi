using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Request
{
    public class MessegeCreateRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string Content { get; set; }
        public int CreatedAt { get; set; }
        public Guid FromId { get; set; }
        public string id1 { get; set; }
        public int MessageTypeId { get; set; }
        public int Status { get; set; }
        public int StatusSend { get; set; }
        public Guid ToId { get; set; }
        public string UserAvatar { get; set; }
    }
}
