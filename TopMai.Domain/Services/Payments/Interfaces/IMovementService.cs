using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IMovementService
    {
        Task<List<Movement>> GetAll(int Page= 1, int Limit = 10);
        Movement Get(Guid id);
        float GetTotalMonthOutputByWalletId(Guid walletId);
        int GetAmountOfPendingMovementInputs();
        int GetAmountOfPendingMovementOutputs();
        Movement Post(Movement movement);
        Task<Movement> Put(Movement newMovement);
        Movement GetPendingInputByUserId(Guid userId);
        List<Movement> GetAllMovementsByUserId(Guid userId);
        Movement GetPendingOutputByUserId(Guid userId);
        List<Movement> GetSolvedInputsByUserId(Guid userId);
        List<Movement> GetSolvedOutputsByUserId(Guid userId);
        Task<Movement> CancelMovement(Guid movementId);
        Task<bool> Delete(Guid id);
    }
}
