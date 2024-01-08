using Common.Utils.Enums;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Transactions;
using TopMai.Domain.DTO.Transactions;
using TopMai.Domain.Services.Transactions.Interfaces;

namespace TopMai.Domain.Services.Transactions
{
    public class TransactionsAccreditationServices : ITransactionsAccreditationServices
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public TransactionsAccreditationServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion


        #region Methods
        public async Task<bool> NewTransactionAcreditation(AddTransactionsAccreditation_Dto add)
        {
            TransactionsAccreditation transactionsAccreditation = new TransactionsAccreditation()
            {
                Id = Guid.NewGuid(),
                IdStatus = (int)Enums.State.EnProceso_Transaction,
                IdTransaction = add.IdTransaction,
                RegistrationDate = DateTime.Now,
                AccreditationDate = DateTime.Now.AddDays(add.Days),
            };
            _unitOfWork.TransactionsAccreditationRepository.Insert(transactionsAccreditation);

            return await _unitOfWork.Save() > 0;
        }
        #endregion
    }
}
