using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Products
{
    public partial class Category
    {
        public Category()
        {
            Attributes = new HashSet<Attribute>();
            Subcategories = new List<Subcategory>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? UrlImage { get; set; }
        public string? IconName { get; set; }
        public int? Index { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Attribute> Attributes { get; set; }
        public virtual List<Subcategory> Subcategories { get; set; }

        [NotMapped]
        public int? Page { get; set; }

        [NotMapped]
        public List<Publication>? Publications { get; set; }
    }
}