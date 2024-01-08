using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopMai.Domain.DTO.SubCategory
{
    public class SubCategoryDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubcategoryId { get; set; }
        public string? UrlImage { get; set; }
        public string? UrlSecondaryImage { get; set; }
        public int? Index { get; set; }
        public int? IndexMostUsed { get; set; }
        public string? IconName { get; set; }
        public bool? Deleted { get; set; }
        public string? UrlInternalImage { get; set; }
    }
}
