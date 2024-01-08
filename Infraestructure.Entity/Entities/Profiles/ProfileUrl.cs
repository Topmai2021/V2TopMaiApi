namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class ProfileUrl
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? ProfileId { get; set; }
        public bool? Deleted { get; set; }

        public virtual Profile? Profile { get; set; }
    }
}