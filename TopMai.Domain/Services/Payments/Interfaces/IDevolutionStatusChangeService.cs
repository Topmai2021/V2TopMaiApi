using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IDevolutionStatusChangeService
    {
        List<DevolutionStatusChange> GetAll();
        DevolutionStatusChange Get(Guid id);
        object Post(DevolutionStatusChange status);
        object Put(DevolutionStatusChange newStatus);
        object Delete(Guid id);
    }
}
