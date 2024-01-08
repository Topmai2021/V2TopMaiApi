using Infraestructure.Core.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Other.Interfaces;
using Version = Infraestructure.Entity.Entities.Other.Version;

namespace TopMai.Domain.Services.Other
{
    public class VersionService : IVersionService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Builder

        public VersionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<Version> GetAll() => _unitOfWork.VersionRepository.GetAll().ToList();

        public Version GetActualVersion(string platform, bool required)
        {
            return _unitOfWork.VersionRepository
                .FindAll(v => v.Platform == platform && v.Required == required 
                                                    && v.Deleted == false)
                .OrderByDescending(v=>v.Number)
                .FirstOrDefault();
           
            // return DBContext.Versions.OrderByDescending(v => v.Number).FirstOrDefault(u => u.Deleted == false && u.Platform == platform && u.Required == required);

        }

        public Version Get(Guid id) => _unitOfWork.VersionRepository.FirstOrDefault(u => u.Id == id);
        
        /**{
            return DBContext.Versions.FirstOrDefault(u => u.Id == id);

        }
        */

        public async Task<object> Post(Version version)
        {

            if (NameIsRepeated(version.Number)) return new { error = "La versión ya existe en el sistema" };

            version.Id = Guid.NewGuid();
            version.Deleted = false;
            version.DateTime = DateTime.Now;

            _unitOfWork.VersionRepository.Insert(version);
            await _unitOfWork.Save();

            return _unitOfWork.VersionRepository.FirstOrDefault(v=>v.Id == version.Id);


        }

        public async Task<object> Put(Version newVersion)
        {
            var idVersion = newVersion.Id;
            if (idVersion == null || idVersion.ToString().Length < 6) return new { error = "Ingrese un id de rol válido " };


            Version? version = _unitOfWork.VersionRepository.FirstOrDefault(v => v.Id == idVersion && newVersion.Id != null);
            //Version? version = DBContext.Versions.Where(r => r.Id == idVersion && newVersion.Id != null).FirstOrDefault();
            if (version == null) return new { error = "El id no coincide con ningun rol " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newVersion.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newVersion) != null && propertyInfo.GetValue(newVersion).ToString() != "")
                {
                    propertyInfo.SetValue(version, propertyInfo.GetValue(newVersion));

                }

            }

            _unitOfWork.VersionRepository.Update(version);
            await _unitOfWork.Save();
            //DBContext.Entry(version).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return version;

        }

        public async Task<object> Delete(Guid id)
        {

            Version version = _unitOfWork.VersionRepository.FirstOrDefault(u => u.Id == id);
            if (version == null) return new { error = "El id ingresado no es válido" };
            version.Deleted = true;
            _unitOfWork.VersionRepository.Update(version);
            await _unitOfWork.Save();
            //DBContext.Entry(version).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return version;
        }

        public bool NameIsRepeated(string number)
        {
            var repeatName = (Version?)_unitOfWork.VersionRepository.FirstOrDefault(r => r.Number == number);
            if (repeatName != null) return true;
            return false;

        }

        #endregion

    }
}
