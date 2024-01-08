using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IPaymentService
    {
        List<Payment> GetAll();
        Payment Get(Guid id);
        object Post(Payment payment);
        object Put(Payment newPayment);
        object Delete(Guid id);
        List<Payment> GetPaymentsByUser(Guid id);
        

    }
}
