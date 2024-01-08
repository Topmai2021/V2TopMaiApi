using Infraestructure.Entity.Entities.Users;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Pin
    {
        public Guid Id { get; set; }
        public string? Value { get; set; }
        public bool? Deleted { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
    }
}