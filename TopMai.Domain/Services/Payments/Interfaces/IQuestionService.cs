using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IQuestionService
    {
        List<Question> GetAll();
        Question Get(Guid id);

        Task<object> Post(Question question);

        Task<object> Put(Question newQuestion);

        Task<object> Delete(Guid id);
    }
}
