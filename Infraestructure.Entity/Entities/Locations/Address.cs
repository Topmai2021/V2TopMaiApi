using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Locations
{
    public partial class Address
    {
        public Address()
        {
            SellRequests = new HashSet<SellRequest>();
        }

        public Guid Id { get; set; }
        public bool? Active { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
        public bool? Deleted { get; set; }
        public int? DepartmentNumber { get; set; }
        public string? Neighborhood { get; set; }
        public int? Number { get; set; }
        public string? Street { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? PostalCode { get; set; }
        public Guid? ProfileId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Mail { get; set; }
        public string? Phone { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual ICollection<SellRequest> SellRequests { get; set; }
    }
}