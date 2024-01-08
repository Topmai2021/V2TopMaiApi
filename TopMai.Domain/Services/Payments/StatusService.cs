using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopMai.Domain.Services.Payments.Interfaces;

namespace TopMai.Domain.Services.Payments
{
    public class StatusService : IStatusService
    {
        #region Attributes
        private DataContext _dBContext;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public StatusService(DataContext dBContext, IUnitOfWork unitOfWork)
        {
            _dBContext = dBContext;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public List<Status> GetAll() => _dBContext.Statuses.OrderByDescending(x => x.Id).ToList();

        public Status Get(int id) => _unitOfWork.StatusRepository.FirstOrDefault(u => u.Id == id);

        public async Task<Status> Post(Status status)
        {
            if (status.Name == null || status.Name.Length < 3)
                throw new BusinessException("El nombre del status debe ser al menos 3 caracteres");

            if (!Regex.Match(status.Name, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$").Success)
                throw new BusinessException("El nombre no puede tener caracteres especiales");

            if (NameIsRepeated(status.Name, status.Ambit))
                throw new BusinessException("El status ya existe en el sistema");


            status.Id = IdStatus();
            status.Deleted = false;
            _unitOfWork.StatusRepository.Insert(status);
            await _unitOfWork.Save();

            return status;
        }

        public async Task<Status> Put(Status newStatus)
        {
            var status = _unitOfWork.StatusRepository.FirstOrDefault(x => x.Id == newStatus.Id);
            if (status == null)
                throw new BusinessException("El id ingresado no es válido");
            status.Deleted = newStatus.Deleted;
            status.Ambit = newStatus.Ambit;
            status.Name = newStatus.Name;
            _unitOfWork.StatusRepository.Update(status);
            await _unitOfWork.Save();

            return status;
        }

        public async Task<bool> Delete(int id)
        {
            var status = _unitOfWork.StatusRepository.FirstOrDefault(x => x.Id == id);
            if (status == null)
                throw new BusinessException("El id ingresado no es válido");

            status.Deleted = true;
            _unitOfWork.StatusRepository.Update(status);

            return await _unitOfWork.Save() > 0;
        }

        private bool NameIsRepeated(string name, string ambit)
        {
            var repeatName = (Status?)_dBContext.Statuses.Where(r => r.Name.ToLower() == name.ToLower()
                                                                  && r.Ambit.ToLower() == ambit.ToLower()).FirstOrDefault();
            if (repeatName != null)
                return true;

            return false;
        }

        private int IdStatus()
        {
            int id = 0;
            try
            {
                id = _unitOfWork.StatusRepository.GetAll().Max(x => x.Id) + 1;
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
