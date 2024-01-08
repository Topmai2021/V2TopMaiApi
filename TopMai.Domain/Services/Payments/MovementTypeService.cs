using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Payments
{
    public class MovementTypeService : IMovementTypeService
    {
        #region Attributes
        private DataContext _dBContext;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public MovementTypeService(DataContext dBContext, IUnitOfWork unitOfWork)
        {
            _dBContext = dBContext;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods
        public List<MovementType> GetAll() => _unitOfWork.MovementTypeRepository.GetAll().OrderByDescending(x => x.Id).ToList();

        public MovementType Get(int id) => _unitOfWork.MovementTypeRepository.FirstOrDefault(u => u.Id == id);

        public async Task<MovementType> Post(MovementType movementType)
        {
            if (movementType.Name == null || movementType.Name.Length < 3)
                throw new BusinessException("El nombre del rol debe ser al menos 3 caracteres");

            if (!Regex.Match(movementType.Name, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$").Success)
                throw new BusinessException("El nombre no puede tener caracteres especiales");


            if (NameIsRepeated(movementType.Name))
                throw new BusinessException("El rol ya existe en el sistema");

            movementType.Id = IdMovementType();
            _unitOfWork.MovementTypeRepository.Insert(movementType);
            await _unitOfWork.Save();

            return movementType;
        }

        public async Task<MovementType> Put(MovementType newMovementType)
        {
            MovementType movementType = Get(newMovementType.Id);
            if (movementType == null)
                throw new BusinessException("No se encuentra el Tipo de Movimiento a Editar");

            movementType.Name = newMovementType.Name;
            movementType.Deleted = newMovementType.Deleted;
            _unitOfWork.MovementTypeRepository.Update(movementType);
            await _unitOfWork.Save();

            return movementType;
        }

        public async Task<bool> Delete(int id)
        {
            MovementType movementType = Get(id);
            if (movementType == null)
                throw new BusinessException("No se encuentra el Tipo de Movimiento a Eliminar");

            movementType.Deleted = true;
            _unitOfWork.MovementTypeRepository.Update(movementType);

            return await _unitOfWork.Save() > 0;
        }

        private bool NameIsRepeated(string name)
        {
            var repeatName = (MovementType?)_dBContext.MovementTypes.FirstOrDefault(r => r.Name == name);
            if (repeatName != null)
                return true;

            return false;
        }

        private int IdMovementType()
        {
            int id = 0;
            try
            {
                id = _unitOfWork.MovementTypeRepository.GetAll().Max(x => x.Id) + 1;
            }
            catch (Exception)
            {
                id = 1;
            }

            return id;
        }
        #endregion
    }
}
