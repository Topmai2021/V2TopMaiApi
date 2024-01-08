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
using bank = Infraestructure.Entity.Entities.Payments.Bank;

namespace TopMai.Domain.Services.Other
{
    public class BankService : IBankService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Builder

        public BankService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<bank> GetAll() => _unitOfWork.BankRepository.GetAll().ToList();

    

        public bank Get(Guid id) => _unitOfWork.BankRepository.FirstOrDefault(u => u.Id == id);
    

        public async Task<object> Post(Bank bank)
        {


            bank.Id = Guid.NewGuid();
            bank.Deleted = false;
            if(bank.Name == null)
                return new { error = "Nombre no válido" };
            

            _unitOfWork.BankRepository.Insert(bank);
            await _unitOfWork.Save();

            return _unitOfWork.BankRepository.FirstOrDefault(v=>v.Id == bank.Id);


        }
        
        public async Task<object> Put(Bank newBank)
        {
            var idBank = newBank.Id;
            if (idBank == null || idBank.ToString().Length < 6) return new { error = "Ingrese un id de banco válido " };


            Bank bank = _unitOfWork.BankRepository.FirstOrDefault(v => v.Id == idBank && newBank.Id != null);
            //bank? bank = DBContext.Banks.Where(r => r.Id == idBank && newBank.Id != null).FirstOrDefault();
            if (bank == null) return new { error = "El id no coincide con ningun banco " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newBank.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newBank) != null && propertyInfo.GetValue(newBank).ToString() != "")
                {
                    propertyInfo.SetValue(bank, propertyInfo.GetValue(newBank));

                }

            }

            _unitOfWork.BankRepository.Update(bank);
            await _unitOfWork.Save();
            //DBContext.Entry(bank).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return bank;

        }

        public async Task<object> Delete(Guid id)
        {

            Bank bank = _unitOfWork.BankRepository.FirstOrDefault(u => u.Id == id);
            if (bank == null) return new { error = "El id ingresado no es válido" };
            bank.Deleted = true;
            _unitOfWork.BankRepository.Update(bank);
            await _unitOfWork.Save();
            //DBContext.Entry(bank).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return bank;
        }

              

        #endregion

    }
}
