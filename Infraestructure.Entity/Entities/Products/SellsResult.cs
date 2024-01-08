using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructure.Entity.Entities.Payments;

namespace Infraestructure.Entity.Entities.Products
{
    public class SellsResult
    {
        public int totalPages { get; set; }
        public int pageNumber { get; set; }
        public long TotalRecords { get; set; }
        public List<Sell> sells { get; set; }

    }
}
