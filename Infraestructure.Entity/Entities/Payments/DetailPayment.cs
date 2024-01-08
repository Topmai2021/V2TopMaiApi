using Infraestructure.Entity.Entities.Products;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class DetailPayment
    {
        public Guid Id { get; set; }
        public float? Amount { get; set; }
        public Guid? PublicationId { get; set; }
        public Guid? PaymentId { get; set; }
        public bool? Deleted { get; set; }

        public virtual Payment? Payment { get; set; }
        public virtual Publication? Publication { get; set; }
    }
}