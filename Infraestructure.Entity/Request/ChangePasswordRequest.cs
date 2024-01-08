namespace Infraestructure.Entity.Request
{
    public class ChangePasswordRequest
    {
        public Guid? id { get; set; }
        public string lastPassword { get; set; }
        public string newPassword { get; set; }
    }
}