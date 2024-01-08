using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IDetailPaymentService
    {
        List<DetailPayment> GetAll();
        DetailPayment Get(Guid id);
        object Post(DetailPayment detailPayment);
        object Put(DetailPayment newDetailPayment);
        object Delete(Guid id);
    }
}
