namespace Infraestructure.Entity.Entities.Locations
{
    public partial class City
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? ProvinceId { get; set; }
        public bool? Deleted { get; set; }

        public virtual Province? Province { get; set; }
    }
}