using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.RegularExpressions;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Payments
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Builder
        public CurrencyService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public List<Currency> GetAll() => _unitOfWork.CurrencyRepository.GetAll().ToList();

        public Currency Get(int id) => _unitOfWork.CurrencyRepository.FirstOrDefault(u => u.Id == id);

        public async Task<Currency> Post(Currency currency)
        {
            if (currency.Name == null || currency.Name.Length < 3)
                throw new BusinessException("El nombre de la divisa debe ser de al menos 3 caracteres");

            if (!Regex.Match(currency.Name, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$").Success)
                throw new BusinessException("El nombre no puede tener caracteres especiales");

            if (currency.Abbreviation == null || currency.Abbreviation.Length < 3)
                throw new BusinessException("El nombre de la abreviatura debe ser de al menos 2 caracteres");
            
            currency.Id = IdCurrency();
            _unitOfWork.CurrencyRepository.Insert(currency);
            await _unitOfWork.Save();

            return currency;
        }

        public async Task<Currency> Put(Currency newCurrency)
        {
            Currency currency = Get(newCurrency.Id);
            if (currency == null)
                throw new BusinessException("El id no coincide con ninguna divisa");

            currency.Abbreviation = newCurrency.Abbreviation;
            currency.Name = newCurrency.Name;
            currency.Deleted = newCurrency.Deleted;
            _unitOfWork.CurrencyRepository.Update(currency);
            await _unitOfWork.Save();

            return currency;
        }

        public async Task<bool> Delete(int id)
        {
            Currency currency = Get(id);
            if (currency == null)
                throw new BusinessException("El id no coincide con ninguna divisa");

            currency.Deleted = true;
            _unitOfWork.CurrencyRepository.Update(currency);

            return await _unitOfWork.Save() > 0;
        }

        private int IdCurrency()
        {
            int id = 0;
            try
            {
                id = _unitOfWork.CurrencyRepository.GetAll().Max(x => x.Id) + 1;
            }
            catch (Exception)
            {
                id = 1;
            }

            return id;
        }
        #endregion
    }
}
