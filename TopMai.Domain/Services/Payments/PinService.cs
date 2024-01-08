using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using NETCore.Encrypt;
using System.Reflection;
using TopMai.Domain.Services.Payments.Interfaces;
using pin = Infraestructure.Entity.Entities.Payments.Pin;

namespace TopMai.Domain.Services.Other
{
    public class PinService : IPinService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Builder

        public PinService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<pin> GetAll() => _unitOfWork.PinRepository.GetAll(x => x.User).ToList();

        public pin Get(Guid id) => _unitOfWork.PinRepository.FirstOrDefault(u => u.Id == id);


        public async Task<object> Post(Pin pin)
        {


            pin.Id = Guid.NewGuid();
            pin.Deleted = false;
            if (pin.Value == null || pin.Value.Length < 4)
                return new { error = "Pin no válido" };
            pin.Value = EncryptProvider.Sha1(pin.Value);

            _unitOfWork.PinRepository.Insert(pin);
            await _unitOfWork.Save();

            return _unitOfWork.PinRepository.FirstOrDefault(v => v.Id == pin.Id);


        }

        public async Task<object> Put(Pin newPin)
        {
            var idPin = newPin.Id;
            if (idPin == null || idPin.ToString().Length < 6) return new { error = "Ingrese un id de banco válido " };


            Pin pin = _unitOfWork.PinRepository.FirstOrDefault(v => v.Id == idPin && newPin.Id != null);
            //pin? pin = DBContext.Pins.Where(r => r.Id == idPin && newPin.Id != null).FirstOrDefault();
            if (pin == null) return new { error = "El id no coincide con ningun banco " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newPin.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newPin) != null && propertyInfo.GetValue(newPin).ToString() != "")
                {
                    propertyInfo.SetValue(pin, propertyInfo.GetValue(newPin));

                }

            }

            _unitOfWork.PinRepository.Update(pin);
            await _unitOfWork.Save();
            //DBContext.Entry(pin).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return pin;

        }

        public bool ValidatePin(Guid userId, string pin)
        {
            string encriptedPin = EncryptProvider.Sha1(pin);
            Pin pinFinded = _unitOfWork.PinRepository.FirstOrDefault
                (p => p.UserId == userId && p.Value == encriptedPin && p.Deleted != true);
            if (pinFinded == null)
            {
                return false;
            }
            return true;

        }
        public async Task<object> Delete(Guid id)
        {

            Pin pin = _unitOfWork.PinRepository.FirstOrDefault(u => u.Id == id);
            if (pin == null) return new { error = "El id ingresado no es válido" };
            pin.Deleted = true;
            _unitOfWork.PinRepository.Update(pin);
            await _unitOfWork.Save();
            //DBContext.Entry(pin).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return pin;
        }



        #endregion

    }
}
