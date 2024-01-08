using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Request
{
    public class UpdateRequestCategory
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? UrlImage { get; set; }
        public string? IconName { get; set; }
        public int? Index { get; set; }

        public bool? Deleted { get; set; }

    }
}
