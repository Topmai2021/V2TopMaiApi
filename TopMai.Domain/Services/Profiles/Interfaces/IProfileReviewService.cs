using Infraestructure.Entity.Entities.Profiles;

namespace TopMai.Domain.Services.Profiles.Interfaces
{
    public interface IProfileReviewService
    {
        List<ProfileReview> GetAll();
        object Get(Guid? id);
        object Post(ProfileReview profileReview);
        object Put(ProfileReview newProfileReview);
        object IsCalificated(Guid? fromId, Guid? toId, Guid? sellId);
        object GetProfileReviewsByProfileId(Guid? id);
        object GetMyValorationToProfileId(Guid id, Guid toId);
        object GetValorationByProfileId(Guid? id);
        Task<bool> Delete(Guid idProfileReview);
    }
}
