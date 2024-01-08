namespace Infraestructure.Entity.Request
{
    public class ChangeRoleRequest
    {
        public Guid idUser { get; set; }
        public int idRole { get; set; }
    }
}