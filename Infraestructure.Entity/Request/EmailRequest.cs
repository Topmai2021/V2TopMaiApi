namespace Infraestructure.Entity.Request
{
    public class EmailRequest
    {
        public string email { get; set; }
        public string? subject { get; set; }
        public string? body { get; set; }
    }
}