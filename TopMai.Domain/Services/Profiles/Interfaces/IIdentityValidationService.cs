using Infraestructure.Entity.Entities.Profiles;

namespace TopMai.Domain.Services.Profiles.Interfaces
{
    public interface IIdentityValidationService
    {
        List<IdentityValidation> GetAll();
        IdentityValidation Get(Guid id);
        Task<object> Post(IdentityValidation identityValidation);
        Task<object> Put(IdentityValidation newIdentityValidation);
        Task<object> AddImageToIdentityValidation(Guid idImage, Guid idIdentityValidation, string? type);

        List<IdentityValidationImage> GetImagesByIdentityValidation(Guid id);
        Task<object> RemoveImagesIdentityValidation(Guid idIdentityValidation);
        Task<object> HasPendingIdentityValidation(Guid profileId);
        Task<object> Delete(Guid id);
    }
}
