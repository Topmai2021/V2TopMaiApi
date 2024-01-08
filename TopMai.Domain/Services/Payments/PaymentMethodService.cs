using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Payments
{
    public class PaymentMethodService : IPaymentMethodService
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public PaymentMethodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public List<PaymentMethod> GetAll()
        {
            List<PaymentMethod> paymentMethods = _unitOfWork.PaymentMethodRepository.FindAll(s => !s.Deleted).OrderByDescending(x => x.Id).ToList();

            return paymentMethods;
        }

        public PaymentMethod Get(int id) => _unitOfWork.PaymentMethodRepository.FirstOrDefault(x => x.Id == id);

        public async Task<PaymentMethod> Post(PaymentMethod paymentMethod)
        {
            paymentMethod.Id = IdPaymentMethod();
            _unitOfWork.PaymentMethodRepository.Insert(paymentMethod);
            await _unitOfWork.Save();

            return paymentMethod;
        }

        public async Task<PaymentMethod> Put(PaymentMethod newPaymentMethod)
        {

            PaymentMethod? paymentMethod = Get(newPaymentMethod.Id);
            if (paymentMethod == null)
                throw new BusinessException("El id no coincide con ningun metodo de pago");

            paymentMethod.Deleted = newPaymentMethod.Deleted;
            paymentMethod.Name = newPaymentMethod.Name;
            paymentMethod.Commission = newPaymentMethod.Commission;
            paymentMethod.BuyerCommission = newPaymentMethod.BuyerCommission;
            paymentMethod.AccreditationDays = newPaymentMethod.AccreditationDays;

            _unitOfWork.PaymentMethodRepository.Update(paymentMethod);
            await _unitOfWork.Save();

            return paymentMethod;
        }

        public async Task<bool> Delete(int id)
        {
            PaymentMethod paymentMethod = Get(id);
            if (paymentMethod == null)
                throw new BusinessException("El id ingresado no es válido");

            paymentMethod.Deleted = true;
            _unitOfWork.PaymentMethodRepository.Update(paymentMethod);

            return await _unitOfWork.Save() > 0;
        }

        private int IdPaymentMethod()
        {
            int id = 0;
            try
            {
                id = _unitOfWork.PaymentMethodRepository.GetAll().Max(x => x.Id) + 1;
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
