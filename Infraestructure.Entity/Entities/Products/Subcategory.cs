using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Products
{
    public partial class Subcategory
    {
        public Subcategory()
        {
            Publications = new HashSet<Publication>();
        }

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

        public virtual Category? Category { get; set; }
        public virtual ICollection<Publication> Publications { get; set; }
        [NotMapped]
        public int TotalCount { get; set; }
    }
}