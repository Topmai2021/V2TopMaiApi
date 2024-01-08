using Infraestructure.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Users
{
    public class Permission : IBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public Role? Role { get; set; }
        public Guid? RoleId { get; set; }

        public Permission()
        {
        }

        ~Permission()
        {
        }
    }//end Permission
}