namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class Medal
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public bool? Deleted { get; set; }
        public string? Name { get; set; }

        public virtual Image? Image { get; set; }
    }
}