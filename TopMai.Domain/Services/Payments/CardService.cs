using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Users;
using NETCore.Encrypt;
using Openpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Other.Interfaces;
using TopMai.Domain.Services.Payments.Interfaces;
using card = Infraestructure.Entity.Entities.Payments.Card;
using Infraestructure.Entity.Entities.Profiles;
using Openpay.Entities;
using Card = Infraestructure.Entity.Entities.Payments.Card;
namespace TopMai.Domain.Services.Other
{
    public class CardService : ICardService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Builder

        public CardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<card> GetAll() => _unitOfWork.CardRepository.GetAll().ToList();

    

        public card Get(Guid id) => _unitOfWork.CardRepository.FirstOrDefault(u => u.Id == id);
    

        public async Task<object> Post(Infraestructure.Entity.Entities.Payments.Card card)
        {


            card.Id = Guid.NewGuid();
            card.Deleted = false;
            
            card.SecurityCode = EncryptProvider.Base64Encrypt(card.SecurityCode).ToString();
            
               Boolean production = false;
            OpenpayAPI openpayAPI = new OpenpayAPI("sk_ed50cb213356462faa3d139c2f27938f", "mfz0rmrjpxpihzlwtimp", production);

            
            Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == card.ProfileId
                                                                && p.Deleted != true);
            if (profile == null)
            {
                return new { error = "El perfil no es válido" };

            }

            if (profile.OpenPayCustomerId == null)
            {
                //create customer
                Customer customer = new Customer();
                customer.Name = profile.Name;
                customer.LastName = profile.LastName;
                customer.PhoneNumber = profile.Phone;
                customer.Email = "topmai.com.mx@gmail.com";
                customer.ExternalId = profile.Id.ToString();
                profile.OpenPayCustomerId = openpayAPI.CustomerService.Create(customer).Id;
                await _unitOfWork.Save();
            }

               // create card
                Openpay.Entities.Card openPayCard = new Openpay.Entities.Card();
                openPayCard.CardNumber = (string)card.Number.ToString();
                openPayCard.BankCode = (string)card.SecurityCode.ToString();
                openPayCard.ExpirationMonth = card.ExpirationMonth.ToString();
                openPayCard.ExpirationYear = card.ExpirationYear.ToString();
                try{
                    openPayCard.Cvv2 =  EncryptProvider.Base64Decrypt(card.SecurityCode.ToString()).ToString();
                    

                }catch(Exception e)
                {
                    openPayCard.Cvv2 = (string)card.SecurityCode.ToString();

                }
                openPayCard.HolderName = card.FullName;
                try
                {
                    card.OpenPayCardId = openpayAPI.CardService.Create(profile.OpenPayCustomerId, openPayCard).Id;
                    await _unitOfWork.Save();

                }
                catch (Exception e)
                {
                    return new { error = e.Message };
                }


            _unitOfWork.CardRepository.Insert(card);
            await _unitOfWork.Save();

            return _unitOfWork.CardRepository.FirstOrDefault(v=>v.Id == card.Id);


        }
        
        public async Task<object> Put(Card newCard)
        {
            var idCard = newCard.Id;
            if (idCard == null || idCard.ToString().Length < 6) return new { error = "Ingrese un id de tarjeta válida " };


            Card card = _unitOfWork.CardRepository.FirstOrDefault(v => v.Id == idCard && newCard.Id != null);
            //card? card = DBContext.Cards.Where(r => r.Id == idCard && newCard.Id != null).FirstOrDefault();
            if (card == null) return new { error = "El id no coincide con ninguna tarjeta " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newCard.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newCard) != null && propertyInfo.GetValue(newCard).ToString() != "")
                {
                    propertyInfo.SetValue(card, propertyInfo.GetValue(newCard));

                }

            }

            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.Save();
            //DBContext.Entry(card).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return card;

        }

        public List<Card> GetCardsByProfile(Guid profileId) => _unitOfWork.CardRepository.GetAll()
                    .Where(c => c.ProfileId == profileId && c.Deleted != true).ToList();


        public async Task<object> Delete(Guid id)
        {

            Card card = _unitOfWork.CardRepository.FirstOrDefault(u => u.Id == id);
            if (card == null) return new { error = "El id ingresado no es válido" };
            card.Deleted = true;
            _unitOfWork.CardRepository.Update(card);
            await _unitOfWork.Save();
            //DBContext.Entry(card).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return card;
        }

              

        #endregion

    }
}
