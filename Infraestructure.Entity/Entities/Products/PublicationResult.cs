using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Entity.Entities.Products
{
    public class PublicationResult
    {
        public int totalPages { get; set; }
        public int pageNumber { get; set; }
        public long TotalRecords { get; set; }
        public List<Publication> publications { get; set; }

    }
}
