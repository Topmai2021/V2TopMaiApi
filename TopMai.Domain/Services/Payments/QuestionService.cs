using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Other.Interfaces;
using TopMai.Domain.Services.Payments.Interfaces;
using question = Infraestructure.Entity.Entities.Payments.Question;

namespace TopMai.Domain.Services.Other
{
    public class QuestionService : IQuestionService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Builder

        public QuestionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<question> GetAll() => _unitOfWork.QuestionRepository.GetAll().ToList();

    

        public Question Get(Guid id) => _unitOfWork.QuestionRepository.FirstOrDefault(u => u.Id == id);
    

        public async Task<object> Post(Question question)
        {


            question.Id = Guid.NewGuid();
            question.Deleted = false;
            if(question.Description == null)
                return new { error = "Descripción no válida" };
            

            _unitOfWork.QuestionRepository.Insert(question);
            await _unitOfWork.Save();

            return _unitOfWork.QuestionRepository.FirstOrDefault(v=>v.Id == question.Id);


        }
        
        public async Task<object> Put(Question newBank)
        {
            var idBank = newBank.Id;
            if (idBank == null || idBank.ToString().Length < 6) return new { error = "Ingrese un id de banco válido " };


            Question question = _unitOfWork.QuestionRepository.FirstOrDefault(v => v.Id == idBank && newBank.Id != null);
            //question? question = DBContext.Banks.Where(r => r.Id == idBank && newBank.Id != null).FirstOrDefault();
            if (question == null) return new { error = "El id no coincide con ningun banco " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newBank.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newBank) != null && propertyInfo.GetValue(newBank).ToString() != "")
                {
                    propertyInfo.SetValue(question, propertyInfo.GetValue(newBank));

                }

            }

            _unitOfWork.QuestionRepository.Update(question);
            await _unitOfWork.Save();
            //DBContext.Entry(question).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return question;

        }

        public async Task<object> Delete(Guid id)
        {

            Question question = _unitOfWork.QuestionRepository.FirstOrDefault(u => u.Id == id);
            if (question == null) return new { error = "El id ingresado no es válido" };
            question.Deleted = true;
            _unitOfWork.QuestionRepository.Update(question);
            await _unitOfWork.Save();
            //DBContext.Entry(question).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return question;
        }

              

        #endregion

    }
}
