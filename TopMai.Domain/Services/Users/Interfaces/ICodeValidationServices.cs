using Common.Utils.Enums;
using TopMai.Domain.DTO.CodeValidation;

namespace TopMai.Domain.Services.Users.Interfaces
{
    public interface ICodeValidationServices
    {
        Task<bool> GenerateCodeEmail(Guid idUser);
        Task<bool> GenerateCodeEmail(string email);
        Task<(bool, string?)> ValidateCode(ValidationCode validate);
        Task<bool> GenerateCodePhone(Guid idUser);
        Task<bool> GenerateCodePhone(string phone);
        Task<bool> ValidateForgottenCode(ChangeForgottenPassword change);
    }
}
