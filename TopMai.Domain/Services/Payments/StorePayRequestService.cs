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
using storePayRequest = Infraestructure.Entity.Entities.Payments.StorePayRequest;

namespace TopMai.Domain.Services.Other
{
    public class StorePayRequestService : IStorePayRequestService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Builder

        public StorePayRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<StorePayRequest> GetAll() => _unitOfWork.StorePayRequestRepository.GetAll().ToList();

    

        public StorePayRequest Get(Guid id) => _unitOfWork.StorePayRequestRepository.FirstOrDefault(u => u.Id == id);
    

        public async Task<object> Post(StorePayRequest storePayRequest)
        {


            storePayRequest.Id = Guid.NewGuid();
            storePayRequest.Deleted = false;
            

            _unitOfWork.StorePayRequestRepository.Insert(storePayRequest);
            await _unitOfWork.Save();

            return _unitOfWork.StorePayRequestRepository.FirstOrDefault(v=>v.Id == storePayRequest.Id);


        }
        
        public async Task<object> Put(StorePayRequest newStorePayRequest)
        {
            var idStorePayRequest = newStorePayRequest.Id;
            if (idStorePayRequest == null || idStorePayRequest.ToString().Length < 6) return new { error = "Ingrese un id de tarjeta válida " };


            StorePayRequest storePayRequest = _unitOfWork.StorePayRequestRepository.FirstOrDefault(v => v.Id == idStorePayRequest && newStorePayRequest.Id != null);
            //storePayRequest? storePayRequest = DBContext.StorePayRequests.Where(r => r.Id == idStorePayRequest && newStorePayRequest.Id != null).FirstOrDefault();
            if (storePayRequest == null) return new { error = "El id no coincide con ninguna tarjeta " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newStorePayRequest.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newStorePayRequest) != null && propertyInfo.GetValue(newStorePayRequest).ToString() != "")
                {
                    propertyInfo.SetValue(storePayRequest, propertyInfo.GetValue(newStorePayRequest));

                }

            }

            _unitOfWork.StorePayRequestRepository.Update(storePayRequest);
            await _unitOfWork.Save();
            //DBContext.Entry(storePayRequest).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return storePayRequest;

        }

        public List<StorePayRequest> GetStorePayRequestsByProfile(Guid profileId) => _unitOfWork.StorePayRequestRepository.GetAll()
                    .Where(c => c.ProfileId == profileId && c.Deleted != true).OrderByDescending(s=>s.DateTime).ToList();


        public async Task<object> Delete(Guid id)
        {

            StorePayRequest storePayRequest = _unitOfWork.StorePayRequestRepository.FirstOrDefault(u => u.Id == id);
            if (storePayRequest == null) return new { error = "El id ingresado no es válido" };
            storePayRequest.Deleted = true;
            _unitOfWork.StorePayRequestRepository.Update(storePayRequest);
            await _unitOfWork.Save();
            //DBContext.Entry(storePayRequest).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return storePayRequest;
        }

              

        #endregion

    }
}
