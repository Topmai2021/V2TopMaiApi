using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IStatusService
    {
        List<Status> GetAll();
        Status Get(int id);
        Task<Status> Post(Status status);
        Task<Status> Put(Status newStatus);
        Task<bool> Delete(int id);
    }
}
