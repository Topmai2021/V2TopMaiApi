namespace Infraestructure.Entity.Entities.Users
{
    public partial class VerifyCode
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Deleted { get; set; }
    }
}