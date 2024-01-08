using Infraestructure.Entity.Entities.Profiles;
using TopMai.Domain.DTO.Profiles;

namespace TopMai.Domain.Services.Profiles.Interfaces
{
	public interface IGenderService
    {
        List<GenderDto> GetAll();
        GenderDto Get(int id);
        object Delete(int id);

		Gender GetByName(string name);
    }
}
