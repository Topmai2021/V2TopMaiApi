using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using TopMai.Domain.DTO.Profiles;
using TopMai.Domain.Services.Profiles.Interfaces;

namespace TopMai.Domain.Services.Profiles
{
    public class GenderService : IGenderService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Builder
        public GenderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public List<GenderDto> GetAll()
        {
            IEnumerable<Gender> genders = _unitOfWork.GenderRepository.GetAll();

            return genders.Select(x => new GenderDto()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
        }

        public GenderDto Get(int id)
        {
            Gender gender = _unitOfWork.GenderRepository.FirstOrDefault(x => x.Id == id);

            return new GenderDto()
            {
                Id = gender.Id,
                Name = gender.Name,
            };
        }
        public object Delete(int id)
        {

            Gender Gender = _unitOfWork.GenderRepository.FirstOrDefault(p => p.Id == id);
            if (Gender == null)
                return new { error = "Seleccione una publicación válida" };

            _unitOfWork.GenderRepository.Delete(Gender);
            var isDelted=_unitOfWork.SaveChanges()>0;

            return new {isDeleted= isDelted };
        }

		public Gender GetByName(string name)
		{
			return _unitOfWork.GenderRepository.FirstOrDefault(x => x.Name == name);
		}
		#endregion
	}
}
