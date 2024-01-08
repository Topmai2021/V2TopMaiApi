using Infraestructure.Core.Data;
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
    public class StatusChangeService : IStatusChangeService
    {
        #region Attributes
        private DataContext _dBContext;
        #endregion

        #region Builder
        public StatusChangeService(DataContext dBContext)
        {
            this._dBContext = dBContext;

        }
        #endregion

        #region Methods
        public List<StatusChange> GetAll() => _dBContext.StatusChanges.OrderByDescending(x => x.Id).ToList();

        public StatusChange Get(Guid id) => _dBContext.StatusChanges.FirstOrDefault(u => u.Id == id);

        public object Post(StatusChange StatusChange)
        {
          

            StatusChange.Id = Guid.NewGuid();
            StatusChange.Deleted = false;

            _dBContext.StatusChanges.Add(StatusChange);
            _dBContext.SaveChanges();

            return _dBContext.StatusChanges.First(r => r.Id == StatusChange.Id);
        }

        public object Put(StatusChange newStatusChange)
        {
            var idStatusChange = newStatusChange.Id;
            if (idStatusChange == null || idStatusChange.ToString().Length < 6)
                return new { error = "Ingrese un id de rol válido " };

            StatusChange? StatusChange = _dBContext.StatusChanges.FirstOrDefault(r => r.Id == idStatusChange && newStatusChange.Id != null);
            if (StatusChange == null)
                return new { error = "El id no coincide con ningun cambio de estado " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newStatusChange.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newStatusChange) != null && propertyInfo.GetValue(newStatusChange).ToString() != "")
                    propertyInfo.SetValue(StatusChange, propertyInfo.GetValue(newStatusChange));
            }

            _dBContext.Entry(StatusChange).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return StatusChange;
        }

        public object Delete(Guid id)
        {
            StatusChange StatusChange = _dBContext.StatusChanges.FirstOrDefault(u => u.Id == id);
            if (StatusChange == null)
                return new { error = "El id ingresado no es válido" };

            StatusChange.Deleted = true;
            _dBContext.Entry(StatusChange).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return StatusChange;
        }
    
        #endregion

}
}

    