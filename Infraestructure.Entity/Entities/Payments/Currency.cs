using Infraestructure.Entity.Entities.Products;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Currency
    {
        public Currency()
        {
            Movements = new HashSet<Movement>();
            Payments = new HashSet<Payment>();
            Publications = new HashSet<Publication>();
            SellRequests = new HashSet<SellRequest>();
            Sells = new HashSet<Sell>();
            Wallets = new HashSet<Wallet>();
        }

        public string Abbreviation { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Movement> Movements { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Publication> Publications { get; set; }
        public virtual ICollection<SellRequest> SellRequests { get; set; }
        public virtual ICollection<Sell> Sells { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}