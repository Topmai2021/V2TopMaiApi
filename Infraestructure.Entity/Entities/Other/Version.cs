namespace Infraestructure.Entity.Entities.Other
{
    public partial class Version
    {
        public Guid Id { get; set; }
        public string? Number { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? Required { get; set; }
        public string? Platform { get; set; }
        public bool? Deleted { get; set; }
    }
}