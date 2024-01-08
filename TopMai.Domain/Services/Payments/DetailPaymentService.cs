using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Payments
{
    public class DetailPaymentService : IDetailPaymentService
    {
        private DataContext DBContext;
        #region Builder
        public DetailPaymentService(DataContext dBContext)
        {
            this.DBContext = dBContext;
        }
        #endregion

        #region Methods
        public List<DetailPayment> GetAll()
        {
            List<DetailPayment> detailPayments = DBContext.DetailPayments.OrderByDescending(x => x.Id).ToList();

            return detailPayments;
        }

        public DetailPayment Get(Guid id)
        {
            return DBContext.DetailPayments.FirstOrDefault(u => u.Id == id);

        }

        public object Post(DetailPayment detailPayment)
        {
            detailPayment.Id = Guid.NewGuid();
            detailPayment.Deleted = false;

            DBContext.DetailPayments.Add(detailPayment);
            DBContext.SaveChanges();

            return DBContext.DetailPayments.Where(r => r.Id == detailPayment.Id).First();
        }

        public object Put(DetailPayment newDetailPayment)
        {
            var idDetailPayment = newDetailPayment.Id;
            if (idDetailPayment == null || idDetailPayment.ToString().Length < 6) return new { error = "Ingrese un id de detalle de pago válido " };

            DetailPayment? detailPayment = DBContext.DetailPayments.Where(r => r.Id == idDetailPayment && newDetailPayment.Id != null).FirstOrDefault();
            if (detailPayment == null) return new { error = "El id no coincide con ningun detalle pago " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newDetailPayment.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newDetailPayment) != null && propertyInfo.GetValue(newDetailPayment).ToString() != "")
                {
                    propertyInfo.SetValue(detailPayment, propertyInfo.GetValue(newDetailPayment));

                }

            }

            DBContext.Entry(detailPayment).State = EntityState.Modified;
            DBContext.SaveChanges();
            return detailPayment;
        }

        public object Delete(Guid id)
        {

            DetailPayment detailPayment = DBContext.DetailPayments.FirstOrDefault(u => u.Id == id);
            if (detailPayment == null) return new { error = "El id ingresado no es válido" };
            detailPayment.Deleted = true;
            DBContext.Entry(detailPayment).State = EntityState.Modified;
            DBContext.SaveChanges();
            return detailPayment;
        }
        #endregion
    }
}
