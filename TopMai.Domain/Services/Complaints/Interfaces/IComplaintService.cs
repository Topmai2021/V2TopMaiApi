using Infraestructure.Entity.Entities.Complaints;

namespace TopMai.Domain.Services.Complaints.Interfaces
{
    public interface IComplaintService
    {
        List<Complaint> GetAll();

        Complaint Get(Guid id);
        object Post(Complaint complaint);
        object Put(Complaint newComplaint);
        object Delete(Guid id);
    }
}
