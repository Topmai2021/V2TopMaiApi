using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.DTO.Transactions.HistoricalTransactions;
using TopMai.Domain.Services.Transactions.Interfaces;

namespace TopMai.Domain.Services.Transactions
{
    public class HistoricalTransactionServices : IHistoricalTransactionServices
    {
        #region Attributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public HistoricalTransactionServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods

        public async Task<bool> InsertHistoricalTransaction(AddHistoricalTransaction_Dto historical)
        {
            HistoricalTransaction historicalTransaction = new HistoricalTransaction()
            {
                Id = Guid.NewGuid(),
                Amount= historical.Amount,
                Observation= historical.Observation,
                IdStatusTransaction = historical.IdStatusTransaction,
                IdTransaction = historical.IdTransaction,
                RegistrationDate = DateTime.Now
            };
            _unitOfWork.HistoricalTransactionRepository.Insert(historicalTransaction);

            return await _unitOfWork.Save() > 0;
        }

        public List<ConsultHistoricalTransactions_Dto> GetAllHistoricalTransacions(Guid IdTransaction)
        {
            var transactions = _unitOfWork.HistoricalTransactionRepository.FindAll(x => x.IdTransaction == IdTransaction,
                                                                                 s => s.IdStatusTransactionNavigation);

            List<ConsultHistoricalTransactions_Dto> result = transactions.Select(x => new ConsultHistoricalTransactions_Dto()
            {
                Status = x.IdStatusTransactionNavigation.Name,
                RegistrationDate = x.RegistrationDate,
                Observation = x.Observation
            }).ToList();

            return result;
        }


        #endregion
    }
}
