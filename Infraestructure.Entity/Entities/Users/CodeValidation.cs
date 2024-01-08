namespace Infraestructure.Entity.Entities.Users
{
    public class CodeValidation
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public int Code { get; set; }
        public DateTime StartDate { get; set; }
        public int TypeCode { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
