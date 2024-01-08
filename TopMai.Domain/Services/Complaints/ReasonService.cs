using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Complaints;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Complaints.Interfaces;

namespace TopMai.Domain.Services.Complaints
{
    public class ReasonService : IReasonService
    {
        #region Attributes
        private DataContext _dBContext;
        #endregion

        #region Builder
        public ReasonService(DataContext dBContext)
        {
            _dBContext = dBContext;
        }
        #endregion

        #region Services
        public List<Reason> GetAll() => _dBContext.Reasons.Where(r => r.Deleted != true).OrderByDescending(x => x.Id).ToList();

        public Reason Get(Guid id) => _dBContext.Reasons.FirstOrDefault(u => u.Id == id);

        public object Post(Reason reason)
        {
            if (reason.Name == null)
                return new { error = "El nombre no es válido" };

            Reason repeated = _dBContext.Reasons.Where(p => p.Name == reason.Name).FirstOrDefault();
            if (repeated != null)
                return new { error = "El nombre ya existe" };

            reason.Id = Guid.NewGuid();
            reason.Deleted = false;

            _dBContext.Reasons.Add(reason);
            _dBContext.SaveChanges();

            return _dBContext.Reasons.First(r => r.Id == reason.Id);
        }

        public object Put(Reason newReason)
        {
            var idReason = newReason.Id;
            if (idReason == null || idReason.ToString().Length < 6) return new { error = "Ingrese un id de rol válido " };

            Reason? reason = _dBContext.Reasons.Where(r => r.Id == idReason && newReason.Id != null).FirstOrDefault();
            if (reason == null) return new { error = "El id no coincide con ningun rol " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newReason.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newReason) != null && propertyInfo.GetValue(newReason).ToString() != "")
                    propertyInfo.SetValue(reason, propertyInfo.GetValue(newReason));
            }

            _dBContext.Entry(reason).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return reason;
        }

        public object Delete(Guid id)
        {
            Reason reason = _dBContext.Reasons.FirstOrDefault(u => u.Id == id);
            if (reason == null)
                return new { error = "El id ingresado no es válido" };

            reason.Deleted = true;
            _dBContext.Entry(reason).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return reason;
        } 
        #endregion
    }
}
