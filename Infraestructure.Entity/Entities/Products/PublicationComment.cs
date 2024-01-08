using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Products
{
    public partial class PublicationComment
    {
        public Guid Id { get; set; }
        public Guid FromId { get; set; }
        public Guid PublicationId { get; set; }
        public Guid? AnsweredPublicationCommentId { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; } = null!;

        public virtual Profile From { get; set; } = null!;
        public virtual Publication Publication { get; set; } = null!;

        [NotMapped]
        public ProfileReview? ProfileReview { get; set; }
    }
}