using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IMovementTypeService
    {
        List<MovementType> GetAll();
        MovementType Get(int id);
        Task<MovementType> Post(MovementType movementType);
        Task<MovementType> Put(MovementType newMovementType);
        Task<bool> Delete(int id);
    }
}
