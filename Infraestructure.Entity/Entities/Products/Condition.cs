using Infraestructure.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Products
{
    public class Condition : IBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public bool? Deleted { get; set; }

        public Condition()
        {
        }

        ~Condition()
        {
        }
    }//end
}