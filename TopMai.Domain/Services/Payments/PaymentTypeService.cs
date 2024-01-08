using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.RegularExpressions;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Payments
{
    public class PaymentTypeService : IPaymentTypeService
    {
        #region Attributes
        private DataContext _dBContext;
        #endregion

        #region Builder
        public PaymentTypeService(DataContext dBContext)
        {
            _dBContext = dBContext;

        }
        #endregion

        #region Methods
        //public List<PaymentType> GetAll() => _dBContext.PaymentTypes.OrderByDescending(x => x.Id).ToList();
        public List<PaymentType> GetAll() => new List<PaymentType>();

        //public PaymentType Get(Guid id) => _dBContext.PaymentTypes.FirstOrDefault(u => u.Id == id);
        public PaymentType Get(Guid id) => new PaymentType();

        public object Post(PaymentType paymentType)
        {
            //if (paymentType.Name == null || paymentType.Name.Length < 3)
            //    return new { error = "El nombre del rol debe ser al menos 3 caracteres" };

            //if (!Regex.Match(paymentType.Name, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$").Success)
            //    return new { error = "El nombre no puede tener caracteres especiales" };

            //if (NameIsRepeated(paymentType.Name))
            //    return new { error = "El rol ya existe en el sistema" };

            //paymentType.Id = Guid.NewGuid();
            //paymentType.Deleted = false;

            //_dBContext.PaymentTypes.Add(paymentType);
            //_dBContext.SaveChanges();

            //return _dBContext.PaymentTypes.Where(r => r.Id == paymentType.Id).First();
            return true;
        }

        public object Put(PaymentType newPaymentType)
        {
            //var idPaymentType = newPaymentType.Id;
            //if (idPaymentType == null || idPaymentType.ToString().Length < 6)
            //    return new { error = "Ingrese un id de rol válido " };

            //PaymentType? paymentType = _dBContext.PaymentTypes.FirstOrDefault(r => r.Id == idPaymentType && newPaymentType.Id != null);
            //if (paymentType == null) return new { error = "El id no coincide con ningun rol " };

            ////loop through each attribute entered and modify it

            //foreach (PropertyInfo propertyInfo in newPaymentType.GetType().GetProperties())
            //{
            //    if (propertyInfo.GetValue(newPaymentType) != null && propertyInfo.GetValue(newPaymentType).ToString() != "")
            //        propertyInfo.SetValue(paymentType, propertyInfo.GetValue(newPaymentType));
            //}

            //_dBContext.Entry(paymentType).State = EntityState.Modified;
            //_dBContext.SaveChanges();

            //return paymentType;

            return true;
        }

        public object Delete(Guid id)
        {

            //PaymentType paymentType = _dBContext.PaymentTypes.FirstOrDefault(u => u.Id == id);
            //if (paymentType == null)
            //    return new { error = "El id ingresado no es válido" };

            //paymentType.Deleted = true;
            //_dBContext.Entry(paymentType).State = EntityState.Modified;
            //_dBContext.SaveChanges();

            //return paymentType;

            return true;
        }

        private bool NameIsRepeated(string name)
        {
            //var repeatName = (PaymentType?)_dBContext.PaymentTypes.FirstOrDefault(r => r.Name == name);
            //if (repeatName != null)
            //    return true;

            return false;
        }
        #endregion
    }
}
