using Infraestructure.Entity.Entities.Profiles;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Devolution
    {
        public Devolution()
        {
            DevolutionStatusChanges = new List<DevolutionStatusChange>();
        }

        public Guid Id { get; set; }

        public Profile? CreatedBy { get; set; }
        public Guid? SellId { get; set; }
        public DateTime? DateTime { get; set; }
        public bool? Deleted { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? AuthorizedById { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int StatusId { get; set; }

        public virtual Sell? Sell { get; set; }
        public virtual Status Status { get; set; } = null!;
        public virtual List<DevolutionStatusChange> DevolutionStatusChanges { get; set; }
    }
}