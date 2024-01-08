using Common.Utils.Exceptions;
using Common.Utils.Helpers;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TopMai.Domain.Services.Users.Interfaces;

namespace TopMai.Domain.Services.Users
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Builder
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public List<Role> GetAll() => _unitOfWork.RoleRepository.FindAll(x => !x.Deleted).ToList();

        public Role Get(int id) => _unitOfWork.RoleRepository.FirstOrDefault(u => u.Id == id);

        public async Task<int> Post(Role role)
        {
            Helper.ValidateName(role.Name);
            if (NameIsRepeated(role.Name))
                throw new BusinessException("El rol ya existe en el sistema");

            role.Id = IdRol();
            _unitOfWork.RoleRepository.Insert(role);
            await _unitOfWork.Save();
            return role.Id;
        }

        public async Task<int> Put(Role newRole)
        {
            Helper.ValidateName(newRole.Name);

            Role role = Get(newRole.Id);
            if (role == null)
                throw new BusinessException("No se encuentra el rol a actualizar.");

            role.Name = newRole.Name;
            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.Save();

            return role.Id;
        }

        public async Task<bool> Delete(int id)
        {
            Role role = Get(id);
            if (role == null)
                throw new BusinessException("No se encuentra el rol a actualizar.");

            role.Deleted = false;
            _unitOfWork.RoleRepository.Update(role);

            return await _unitOfWork.Save() > 0;
        }

        public bool NameIsRepeated(string name)
        {
            var repeatName = _unitOfWork.RoleRepository.FirstOrDefault(u => u.Name.ToLower() == name.ToLower());
            if (repeatName != null)
                return true;

            return false;
        }

        private int IdRol()
        {
            int id = 0;
            try
            {
                id = _unitOfWork.RoleRepository.GetAll().Max(x => x.Id) + 1;
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
