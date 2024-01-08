namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class ReviewType
    {
        public ReviewType()
        {
            ProfileReviews = new HashSet<ProfileReview>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<ProfileReview> ProfileReviews { get; set; }
    }
}