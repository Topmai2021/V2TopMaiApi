using Infraestructure.Entity.Entities.Payments;

namespace TopMai.Domain.Services.Payments.Interfaces
{
    public interface IQuestionAnswerService
    {
        List<QuestionAnswer> GetAll();
        QuestionAnswer Get(Guid id);

        Task<object> Post(QuestionAnswer questionAnswer);

        Task<object> Put(QuestionAnswer newQuestionAnswer);
        Task<object> ValidateAnswer(Guid profileId, string answer,Guid questionId);
        List<Question> GetQuestionsByProfileId(Guid profileId);
        Task<object> Delete(Guid id);
    }
}
