using Infraestructure.Entity.Entities.Complaints;

namespace TopMai.Domain.Services.Complaints.Interfaces
{
    public interface IReasonService
    {
        List<Reason> GetAll();
        Reason Get(Guid id);
        object Post(Reason reason);
        object Put(Reason newReason);
        object Delete(Guid id);
    }
}
