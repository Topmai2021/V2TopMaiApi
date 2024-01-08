namespace Infraestructure.Entity.Entities.Payments
{
    public partial class Bank
    {
        public Bank()
        {
            BankAccounts = new HashSet<BankAccount>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool? Deleted { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}