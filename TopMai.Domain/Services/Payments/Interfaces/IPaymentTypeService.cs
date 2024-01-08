using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IPaymentTypeService
    {
        List<PaymentType> GetAll();
        PaymentType Get(Guid id);
        object Post(PaymentType paymentType);
        object Put(PaymentType newPaymentType);
        object Delete(Guid id);
    }
}
