using Common.Utils.Exceptions;
using Common.Utils.Resources;
using Infraestructure.Core.Dapper;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Users;
using Microsoft.Extensions.Configuration;
using TopMai.Domain.DTO.Bank;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Other
{
    public class BankAccountService : IBankAccountService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        #endregion

        #region Builder

        public BankAccountService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _config = configuration;
        }
        #endregion

        #region Methods
        public List<BankAccount> GetAll() => _unitOfWork.BankAccountRepository.GetAll().ToList();



        public BankAccount Get(Guid id) => _unitOfWork.BankAccountRepository.FirstOrDefault(u => u.Id == id);


        public async Task<object> Post(BankAccount bankAccount)
        {
            bankAccount.Id = Guid.NewGuid();

            bankAccount.IsAdmin = false;

            User user = _unitOfWork.UserRepository.FirstOrDefault(u => u.Id == bankAccount.UserId);
            if (user == null)
                return new { error = "Usuario no válido" };
            int counter = _unitOfWork.BankAccountRepository.GetAll()
                                                        .Where(u => u.UserId == bankAccount.UserId)
                                                        .Count();
            if (counter >= 3)
                return new { error = "Solo se pueden tener 3 cuentas bancarias" };

            if (bankAccount.Active == true)
            {
                var bankAccounts = _unitOfWork.BankAccountRepository.GetAll().Where(u => u.UserId == bankAccount.UserId);
                foreach (var item in bankAccounts)
                {
                    item.Active = false;
                    _unitOfWork.BankAccountRepository.Update(item);
                }
                bankAccount.Active = true;

            }
            else
            {
                bankAccount.Active = false;
            }


            _unitOfWork.BankAccountRepository.Insert(bankAccount);
            await _unitOfWork.Save();

            return _unitOfWork.BankAccountRepository.FirstOrDefault(v => v.Id == bankAccount.Id);


        }

        public List<BankAccount> GetByUser(Guid userId) => _unitOfWork.BankAccountRepository.
                                    FindAll(u => u.UserId == userId, u => u.Bank).ToList();



        public async Task<bool> DefaultBankAccount(Guid userId, Guid idBankAccount)
        {
            List<BankAccount> accounts = _unitOfWork.BankAccountRepository.FindAll(x => x.UserId == userId).ToList();
            if (!accounts.Any())
                throw new BusinessException("El usuario no tiene cuentas asociadas.");

            var account = accounts.FirstOrDefault(x => x.Id == idBankAccount);
            if (account == null)
                throw new BusinessException("El usuario no tiene cuentas asociadas.");

            account.Active = true;
            _unitOfWork.BankAccountRepository.Update(account);

            var accountsDisable = accounts.Where(x => x.Id != idBankAccount).ToList();
            accountsDisable.ForEach(ac =>
            {
                ac.Active = false;
                _unitOfWork.BankAccountRepository.Update(ac);
            });

            return await _unitOfWork.Save() > 0;
        }


        public async Task<ConsultBankAccountDto> GetActualBankAccount()
        {
            DapperHelper.Instancia.ConnectionString = _config.GetConnectionString("DefaultConnection");
            IEnumerable<ConsultBankAccountDto> result = await DapperHelper.Instancia.ExecuteQuerySelect<ConsultBankAccountDto>(StatementSql.getBankTopMai);

            return result.FirstOrDefault()!;
        }

        public async Task<bool> Delete(Guid userId, Guid idBankAccount)
        {
            BankAccount bankAccount = _unitOfWork.BankAccountRepository.FirstOrDefault(u => u.Id == idBankAccount
                                                                                         && u.UserId == userId);
            if (bankAccount == null)
                throw new BusinessException("El usuario no tiene cuentas asociadas.");

            _unitOfWork.BankAccountRepository.Delete(bankAccount);

            return await _unitOfWork.Save() > 0;
        }



        #endregion

    }
}
