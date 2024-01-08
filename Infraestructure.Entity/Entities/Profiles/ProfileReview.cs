using Infraestructure.Entity.Entities.Payments;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Profiles
{
    public partial class ProfileReview
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public int Valoration { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public Guid ReviewTypeId { get; set; }
        public DateTime DateTime { get; set; }
        public Guid SellId { get; set; }

        public virtual ReviewType ReviewType { get; set; } = null!;
        public virtual Sell Sell { get; set; } = null!;
        public virtual Profile To { get; set; } = null!;

        [NotMapped]
        public Profile? From { get; set; }
    }
}