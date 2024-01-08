using Infraestructure.Entity.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IPaymentMethodService
    {
        List<PaymentMethod> GetAll();
        PaymentMethod Get(int id);
        Task<PaymentMethod> Post(PaymentMethod paymentMethod);
        Task<PaymentMethod> Put(PaymentMethod newPaymentMethod);
        Task<bool> Delete(int id);
    }
}
