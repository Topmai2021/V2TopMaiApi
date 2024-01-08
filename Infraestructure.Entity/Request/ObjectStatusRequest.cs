namespace Infraestructure.Entity.Request
{
    public class ObjectStatusRequest
    {
        public Guid id { get; set; }
        public int statusId { get; set; }

        public Guid? userId { get; set; }
    }
}