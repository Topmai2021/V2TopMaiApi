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
    public class DevolutionStatusChangeService : IDevolutionStatusChangeService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public DevolutionStatusChangeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }
        #endregion

        #region Methods
        public List<DevolutionStatusChange> GetAll() => _unitOfWork.DevolutionStatusChangeRepository.GetAll().OrderByDescending(x => x.Id).ToList();

        public DevolutionStatusChange Get(Guid id) => _unitOfWork.DevolutionStatusChangeRepository.FirstOrDefault(u => u.Id == id);

        public object Post(DevolutionStatusChange devolutionStatusChange)
        {
          

            devolutionStatusChange.Id = Guid.NewGuid();
            devolutionStatusChange.Deleted = false;

            _unitOfWork.DevolutionStatusChangeRepository.Insert(devolutionStatusChange);
            _unitOfWork.Save();


            return _unitOfWork.DevolutionStatusChangeRepository
                        .FirstOrDefault(v => v.Id == devolutionStatusChange.Id); 
        }

        public object Put(DevolutionStatusChange newStatusChange)
        {
            var idStatusChange = newStatusChange.Id;
            if (idStatusChange == null || idStatusChange.ToString().Length < 6)
                return new { error = "Ingrese un id de rol válido " };

            DevolutionStatusChange? devolutionStatusChange = _unitOfWork.DevolutionStatusChangeRepository
                                        .FirstOrDefault(u => u.Id == idStatusChange && newStatusChange.Id!=null);
            
            if (devolutionStatusChange == null)
                return new { error = "El id no coincide con ningun estado " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newStatusChange.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newStatusChange) != null && propertyInfo.GetValue(newStatusChange).ToString() != "")
                    propertyInfo.SetValue(devolutionStatusChange, propertyInfo.GetValue(newStatusChange));
            }

            _unitOfWork.DevolutionStatusChangeRepository.Update(devolutionStatusChange);
            _unitOfWork.Save();


            return devolutionStatusChange;
        }

        public object Delete(Guid id)
        {
            DevolutionStatusChange devolutionStatusChange = _unitOfWork.DevolutionStatusChangeRepository
                                                                        .FirstOrDefault(r => r.Id == id);
            if (devolutionStatusChange == null)
                return new { error = "El id ingresado no es válido" };

            devolutionStatusChange.Deleted = true;
            _unitOfWork.DevolutionStatusChangeRepository.Update(devolutionStatusChange);
            _unitOfWork.Save();
            

            return devolutionStatusChange;
        }
    
        #endregion

}
}

    