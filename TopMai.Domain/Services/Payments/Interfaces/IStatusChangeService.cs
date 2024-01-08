using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IStatusChangeService
    {
        List<StatusChange> GetAll();
        StatusChange Get(Guid id);
        object Post(StatusChange status);
        object Put(StatusChange newStatus);
        object Delete(Guid id);
    }
}
