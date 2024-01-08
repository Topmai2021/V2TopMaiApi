using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Payments
{
    public class PaymentService: IPaymentService
    {
        #region Attributes
        private DataContext DBContext; 
        #endregion

        #region Builder
        public PaymentService(DataContext dBContext)
        {
            this.DBContext = dBContext;

        }
        #endregion


        #region Methods

        public List<Payment> GetAll()
        {
            List<Payment> payments = DBContext.Payments.OrderByDescending(x => x.DateHour).ToList();
            foreach (Payment payment in payments)
            {
                payment.From = DBContext.Profiles.Find(payment.FromId);
                payment.To = DBContext.Profiles.Find(payment.ToId);
                payment.Sell = DBContext.Sells.Find(payment.SellId);
                payment.Status = DBContext.Statuses.Find(payment.StatusId);
                payment.PaymentMethod = DBContext.PaymentMethods.Find(payment.PaymentMethodId);
                

            }

            return payments;
        }

        public Payment Get(Guid id)
        {
            Payment payment = DBContext.Payments.Find(id);
            payment.From = DBContext.Profiles.Find(payment.FromId);
            payment.To = DBContext.Profiles.Find(payment.ToId);
            payment.Sell = DBContext.Sells.Find(payment.SellId);
            payment.Status = DBContext.Statuses.Find(payment.StatusId);
            payment.PaymentMethod = DBContext.PaymentMethods.Find(payment.PaymentMethodId);
            
            return payment;

        }

        public object Post(Payment payment)
        {
            payment.Id = Guid.NewGuid();
            payment.Deleted = false;

            DBContext.Payments.Add(payment);
            DBContext.SaveChanges();

            return DBContext.Payments.Where(r => r.Id == payment.Id).First();
        }

        public object Put(Payment newPayment)
        {
            var idPayment = newPayment.Id;
            if (idPayment == null || idPayment.ToString().Length < 6) return new { error = "Ingrese un id de pago válido " };
            
            if(newPayment.StatusId != null)
            {
                var status = DBContext.Statuses.FirstOrDefault(s => s.Id == newPayment.StatusId && s.Ambit == "Payment");
                if (status == null) return new { error = "Ingrese un estado válido" };
            }
            Payment? payment = DBContext.Payments.Where(r => r.Id == idPayment && newPayment.Id != null).FirstOrDefault();
            if (payment == null) return new { error = "El id no coincide con ningun pago " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newPayment.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newPayment) != null && propertyInfo.GetValue(newPayment).ToString() != "")
                    propertyInfo.SetValue(payment, propertyInfo.GetValue(newPayment));

            }

            DBContext.Entry(payment).State = EntityState.Modified;
            DBContext.SaveChanges();

            return payment;

        }

        public List<Payment> GetPaymentsByUser(Guid id)
        {
            List<Payment> payments = DBContext.Payments
            .Include("From")
            .Include("To")
            .Include("Sell")
            .Include("Status")
            .Include("PaymentMethod")
            .Where(r => r.FromId == id || r.ToId == id).OrderByDescending(x => x.DateHour).ToList();
        
            return payments;
        }
        
        public object Delete(Guid id)
        {
            Payment payment = DBContext.Payments.FirstOrDefault(u => u.Id == id);
            if (payment == null) return new { error = "El id ingresado no es válido" };
            payment.Deleted = true;
            DBContext.Entry(payment).State = EntityState.Modified;
            DBContext.SaveChanges();

            return payment;
        }


        #endregion
    }
}
