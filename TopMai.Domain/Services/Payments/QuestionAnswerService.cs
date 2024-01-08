using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Other.Interfaces;
using TopMai.Domain.Services.Payments.Interfaces;
using questionAnswer = Infraestructure.Entity.Entities.Payments.QuestionAnswer;

namespace TopMai.Domain.Services.Other
{
public class QuestionAnswerService : IQuestionAnswerService
{
    #region Attributes
    private IUnitOfWork _unitOfWork;
    #endregion

    #region Builder

    public QuestionAnswerService(IUnitOfWork unitOfWork)
    {
                _unitOfWork = unitOfWork;

    }
    #endregion

    #region Methods
    public List<QuestionAnswer> GetAll() => _unitOfWork.QuestionAnswerRepository.GetAll().ToList();



    public QuestionAnswer Get(Guid id) => _unitOfWork.QuestionAnswerRepository.FirstOrDefault(u => u.Id == id);




    public async Task<object> Post(QuestionAnswer questionAnswer)
    {


                questionAnswer.Id = Guid.NewGuid();
                questionAnswer.Deleted = false;

                Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(u => u.Id == questionAnswer.ProfileId);
                if (profile == null)
                            return new { error = "Perfil no encontrado" };
                Question question = _unitOfWork.QuestionRepository.FirstOrDefault(u => u.Id == questionAnswer.QuestionId);
                if (question == null)
                            return new { error = "Pregunta no encontrada" };

                if (questionAnswer.Answer.Length == 0)
                            return new { error = "La respuesta no puede ser vacia" };
                questionAnswer.Answer = EncryptProvider.Sha1(questionAnswer.Answer);

                QuestionAnswer repeated = _unitOfWork.QuestionAnswerRepository.FirstOrDefault(q=>q.ProfileId == questionAnswer.ProfileId && q.QuestionId == questionAnswer.QuestionId);
                if (repeated != null)
                            return new { error = "Ya existe una respuesta para esta pregunta" };

                _unitOfWork.QuestionAnswerRepository.Insert(questionAnswer);
                await _unitOfWork.Save();

                return _unitOfWork.QuestionAnswerRepository.FirstOrDefault(v => v.Id == questionAnswer.Id);


    }

    public async Task<object> Put(QuestionAnswer newBank)
    {
                var idBank = newBank.Id;
                if (idBank == null || idBank.ToString().Length < 6) return new { error = "Ingrese un id de respuesta válido" };


                QuestionAnswer questionAnswer = _unitOfWork.QuestionAnswerRepository.FirstOrDefault(v => v.Id == idBank && newBank.Id != null);
                //questionAnswer? questionAnswer = DBContext.Banks.Where(r => r.Id == idBank && newBank.Id != null).FirstOrDefault();
                if (questionAnswer == null) return new { error = "El id no coincide con ninguna respuesta " };

                //loop through each attribute entered and modify it

                foreach (PropertyInfo propertyInfo in newBank.GetType().GetProperties())
                {
                            if (propertyInfo.GetValue(newBank) != null && propertyInfo.GetValue(newBank).ToString() != "")
                            {
                                        propertyInfo.SetValue(questionAnswer, propertyInfo.GetValue(newBank));

                            }

                }

                _unitOfWork.QuestionAnswerRepository.Update(questionAnswer);
                await _unitOfWork.Save();
                //DBContext.Entry(questionAnswer).State = EntityState.Modified;
                //DBContext.SaveChanges();
                return questionAnswer;

    }

    public List<Question> GetQuestionsByProfileId(Guid profileId)
    {
                List<Question> questions = _unitOfWork.QuestionRepository.GetAll().ToList();
                List<QuestionAnswer> questionAnswers = _unitOfWork.QuestionAnswerRepository.GetAll().ToList();
                List<Question> questionsByProfile = new List<Question>();

                foreach (Question question in questions)
                {
                            if (questionAnswers.Any(q => q.QuestionId == question.Id && q.ProfileId == profileId))
                            {
                                        questionsByProfile.Add(question);
                            }
                }

                return questionsByProfile;
    }
    
    public async Task<object> ValidateAnswer(Guid profileId, string answer,Guid questionId)
    {

                Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(u => u.Id == profileId);
                if (profile == null)
                            return new { error = "Usuario no encontrado" };
                var answ = EncryptProvider.Sha1(answer);
                QuestionAnswer questionAnswer = _unitOfWork.QuestionAnswerRepository.FirstOrDefault
                        (u => u.ProfileId == profileId 
                            && u.QuestionId == questionId
                            && u.Answer == answer);
                return questionAnswer;

    }
    public async Task<object> Delete(Guid id)
    {

                QuestionAnswer questionAnswer = _unitOfWork.QuestionAnswerRepository.FirstOrDefault(u => u.Id == id);
                if (questionAnswer == null) return new { error = "El id ingresado no es válido" };
                questionAnswer.Deleted = true;
                _unitOfWork.QuestionAnswerRepository.Update(questionAnswer);
                await _unitOfWork.Save();
                //DBContext.Entry(questionAnswer).State = EntityState.Modified;
                //DBContext.SaveChanges();
                return questionAnswer;
    }





    #endregion

}
}
