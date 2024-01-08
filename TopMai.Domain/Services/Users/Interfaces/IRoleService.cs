using Infraestructure.Entity.Entities.Users;

namespace TopMai.Domain.Services.Users.Interfaces
{
    public interface IRoleService
    {
        List<Role> GetAll();
        Role Get(int id);
        Task<int> Post(Role role);
        Task<int> Put(Role newRole);
        Task<bool> Delete(int id);
    }
}
