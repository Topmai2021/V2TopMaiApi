using TopMai.Domain.DTO.Transactions;

namespace TopMai.Domain.Services.Transactions.Interfaces
{
    public interface ITypeOrigenRechargueServices
    {
        List<TypeOrigenRechargue_Dto> GeAlltTypeOrigenRechargue();

        Task<bool> UpdateTypeOrigenRechargue(TypeOrigenRechargue_Dto origen);
    }
}
