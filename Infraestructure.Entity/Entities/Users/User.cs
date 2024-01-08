using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Users
{
    public partial class User
    {
        public User()
        {
            CodeValidations = new HashSet<CodeValidation>();
            SellRequests = new HashSet<SellRequest>();
        }
        

        public Guid Id { get; set; }
        public string Password { get; set; } = null!;
        public bool Validated { get; set; }
        public Guid? ProfileId { get; set; }
        public string? UserName { get; set; }
        public string? Phone { get; set; }
        public string? Mail { get; set; }
        public DateTime RegisterDate { get; set; }
        public short SignupType { get; set; }
        public bool Deleted { get; set; }
        public int RoleId { get; set; }
        public bool VerifiedEmail { get; set; }
        public bool VerifiedPhone { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<CodeValidation> CodeValidations { get; set; }
        public virtual ICollection<SellRequest> SellRequests { get; set; }
        [NotMapped]
        public int TotalCount { get; set; }
    }


}

public class Dashboard
{
    public int PublicationCount { get; set; }
    public int UserCount { get; set; }
    public int ComplaintCount { get; set; }
    public int[] UserCountsByMonth { get; set; }
    public int TotalSells { get; set; }
    public int[] MonthlySells { get; set; }
}