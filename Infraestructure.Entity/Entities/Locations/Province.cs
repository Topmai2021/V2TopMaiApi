namespace Infraestructure.Entity.Entities.Locations
{
    public partial class Province
    {
        public Province()
        {
            Cities = new HashSet<City>();
        }

        public Guid Id { get; set; }
        public Guid? CountryId { get; set; }
        public string? Name { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}