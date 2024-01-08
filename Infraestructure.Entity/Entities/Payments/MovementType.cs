using Infraestructure.Entity.Entities.Transactions;

namespace Infraestructure.Entity.Entities.Payments
{
    public partial class MovementType
    {
        public MovementType()
        {
            Movements = new HashSet<Movement>();
        }

        public string Name { get; set; } = null!;
        public bool Deleted { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Movement> Movements { get; set; }
    }
}