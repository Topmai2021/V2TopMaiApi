namespace Infraestructure.Entity.Entities.Complaints
{
    public partial class Reason
    {
        public Reason()
        {
            Complaints = new HashSet<Complaint>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? IconName { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<Complaint> Complaints { get; set; }
    }
}