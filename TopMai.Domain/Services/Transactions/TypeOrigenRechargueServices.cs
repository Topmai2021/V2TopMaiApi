using Common.Utils.Exceptions;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.DTO.Transactions;
using TopMai.Domain.Services.Transactions.Interfaces;

namespace TopMai.Domain.Services.Transactions
{
    public class TypeOrigenRechargueServices : ITypeOrigenRechargueServices
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public TypeOrigenRechargueServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public List<TypeOrigenRechargue_Dto> GeAlltTypeOrigenRechargue()
        {
            List<TypeOrigenRechargue> list = _unitOfWork.TypeOrigenRechargueRepository.GetAll().ToList();

            List<TypeOrigenRechargue_Dto> result = list.Select(x => new TypeOrigenRechargue_Dto()
            {
                Id = x.Id,
                TypeOrigen = x.TypeOrigen,
            }).ToList();

            return result;
        }


        public async Task<bool> UpdateTypeOrigenRechargue(TypeOrigenRechargue_Dto origen)
        {
            var typeOrigen = _unitOfWork.TypeOrigenRechargueRepository.FirstOrDefault(x => x.Id == origen.Id);
            if (typeOrigen == null)
                throw new BusinessException("No se encontró el recurso a actualizar, por favor verificar.");
            typeOrigen.TypeOrigen = origen.TypeOrigen;

            _unitOfWork.TypeOrigenRechargueRepository.Update(typeOrigen);

            return await _unitOfWork.Save() > 0;
        }
        #endregion
    }
}
