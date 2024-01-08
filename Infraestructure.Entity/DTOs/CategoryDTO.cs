using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infraestructure.Entity.DTOs
{
    public class CategoryDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? UrlImage { get; set; }
        public string? IconName { get; set; }
        public int? Index { get; set; }
        public bool? Deleted { get; set; }
    }
}
