using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IDevolutionService
    {
        List<Devolution> GetAll();
        Devolution Get(Guid id);
        Task<object> Post(Devolution devolution);
        Task<object> Put(Devolution newDevolution);
        Task<object> ChangeDevolutionStatus(Guid devolutionId,int statusId);
        Task<bool> CheckDevolutionStatus();
        Task<object> AcceptDevolution(Guid devolutionId,Guid userId);
        Task<object> DeclineDevolution (Guid devolutionId,Guid userId);
        Task<object> AccreditDevolution(Guid idDevolution);
        object Delete(Guid id);
    }
}
