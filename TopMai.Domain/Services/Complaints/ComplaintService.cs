using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Complaints;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Complaints.Interfaces;

namespace TopMai.Domain.Services.Complaints
{
    public class ComplaintService: IComplaintService
    {
        #region Attributes
        private DataContext _dBContext; 
        #endregion

        #region Builder
        public ComplaintService(DataContext dBContext)
        {
            _dBContext = dBContext;
        }
        #endregion

        #region Methods
        public List<Complaint> GetAll()
        {
            List<Complaint> complaints = _dBContext.Complaints.OrderByDescending(x => x.Id).ToList();
            foreach (var complaint in complaints)
            {
                complaint.Publication = _dBContext.Publications.FirstOrDefault(x => x.Id == complaint.PublicationId);
                complaint.Reason = _dBContext.Reasons.FirstOrDefault(x => x.Id == complaint.ReasonId);
            }

            return complaints;
        }

        public Complaint Get(Guid id) => _dBContext.Complaints.FirstOrDefault(u => u.Id == id);
        public object Post(Complaint complaint)
        {
            //User user = _dBContext.Users.Where(p => p.Id == complaint.UserId).FirstOrDefault();
            //if (user == null) return new { error = "El usuario no es válido" };
            Publication publication = _dBContext.Publications.Where(p => p.Id == complaint.PublicationId).FirstOrDefault();
            if (publication == null)
            {
                User userToReport = _dBContext.Users.Where(p => p.Id == complaint.UserToReportId).FirstOrDefault();
                if (userToReport == null)
                    return new { error = "Debe seleccionar una publicación o usuario a reportar" };
            }

            complaint.Id = Guid.NewGuid();
            complaint.Deleted = false;
            complaint.CreatedAt = DateTime.Now;
            _dBContext.Complaints.Add(complaint);
            _dBContext.SaveChanges();

            return _dBContext.Complaints.First(r => r.Id == complaint.Id);
        }

        public object Put(Complaint newComplaint)
        {
            var idComplaint = newComplaint.Id;
            if (idComplaint == null || idComplaint.ToString().Length < 6) return new { error = "Ingrese un id de rol válido " };

            Complaint? complaint = _dBContext.Complaints.Where(r => r.Id == idComplaint && newComplaint.Id != null).FirstOrDefault();
            if (complaint == null) return new { error = "El id no coincide con ningun rol " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newComplaint.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newComplaint) != null && propertyInfo.GetValue(newComplaint).ToString() != "")
                    propertyInfo.SetValue(complaint, propertyInfo.GetValue(newComplaint));
            }

            _dBContext.Entry(complaint).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return complaint;
        }

        public object Delete(Guid id)
        {
            Complaint complaint = _dBContext.Complaints.FirstOrDefault(u => u.Id == id);
            if (complaint == null)
                return new { error = "El id ingresado no es válido" };

            complaint.Deleted = true;
            _dBContext.Entry(complaint).State = EntityState.Modified;
            _dBContext.SaveChanges();

            return complaint;
        } 
        #endregion
    }
}
