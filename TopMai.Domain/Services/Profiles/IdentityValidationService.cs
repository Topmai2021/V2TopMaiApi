using Common.Utils.Enums;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Other.Interfaces;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Domain.Services.Profiles.Interfaces;
using identityValidation = Infraestructure.Entity.Entities.Profiles.IdentityValidation;

namespace TopMai.Domain.Services.Other
{
    public class IdentityValidationService : IIdentityValidationService
    {
        #region Attributes
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Builder

        public IdentityValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        #endregion

        #region Methods
        public List<identityValidation> GetAll() => _unitOfWork.IdentityValidationRepository
            .GetAll(i => i.Status, p => p.Profile).ToList();



        public identityValidation Get(Guid id) => _unitOfWork.IdentityValidationRepository.FirstOrDefault(u => u.Id == id);


        public async Task<object> Post(IdentityValidation identityValidation)
        {


            identityValidation.Id = Guid.NewGuid();
            identityValidation.Deleted = false;

            IdentityValidation repeated = _unitOfWork.IdentityValidationRepository.FirstOrDefault(v => v.ProfileId == identityValidation.ProfileId
                && v.Deleted != true, i => i.Status, p => p.Profile);
            if (repeated != null)
            {
                if (repeated.Status.Name == "Pendiente")
                {
                    return new { error = "Ya existe una solicitud de validación de identidad pendiente para este perfil" };
                }
                else if (repeated.Status.Name == "Aprobada")
                {
                    if (repeated.Profile.IdentityValidated == true)
                    {
                        return new { error = "Ya existe una solicitud de validación de identidad aprobada para este perfil" };

                    }
                    else
                    {
                        repeated.Profile.IdentityValidated = true;
                        _unitOfWork.ProfileRepository.Update(repeated.Profile);
                        await _unitOfWork.Save();
                        return new { error = "Ya existe una solicitud de validación de identidad aprobada para este perfil, pero el perfil no estaba validado, se ha validado el perfil" };
                    }
                }

            }

            identityValidation.StatusId = (int)Enums.State.Pendiente_IdentityValidation;

            _unitOfWork.IdentityValidationRepository.Insert(identityValidation);
            await _unitOfWork.Save();

            return _unitOfWork.IdentityValidationRepository.FirstOrDefault(v => v.Id == identityValidation.Id);


        }

        public List<IdentityValidationImage> GetImagesByIdentityValidation(Guid id) =>
            _unitOfWork.IdentityValidationImageRepository
                        .GetAll(i => i.Image).Where(i => i.IdentityValidationId == id && i.Deleted != true).ToList();

        public async Task<object> Put(IdentityValidation newIdentityValidation)
        {
            var idIdentityValidation = newIdentityValidation.Id;
            if (idIdentityValidation == null || idIdentityValidation.ToString().Length < 6)
                return new { error = "Ingrese un id de tarjeta válida " };


            IdentityValidation identityValidation = _unitOfWork.IdentityValidationRepository.FirstOrDefault(v => v.Id == idIdentityValidation && newIdentityValidation.Id != null);
            //identityValidation? identityValidation = DBContext.IdentityValidations.Where(r => r.Id == idIdentityValidation && newIdentityValidation.Id != null).FirstOrDefault();
            if (identityValidation == null) return new { error = "El id no coincide con ninguna solicitud de validación " };

            if (newIdentityValidation.StatusId != null)
            {
                Status status = _unitOfWork.StatusRepository.FirstOrDefault(s => s.Id == newIdentityValidation.StatusId);
                if (status == null)
                    return new { error = "Ingrese un estado válido" };

                if (status.Ambit != "IdentityValidation")
                    return new { error = "Ingrese un estado válido" };

                if (status.Name == "Aprobada")
                {
                    Profile profile = _unitOfWork.ProfileRepository
                                .FirstOrDefault(p => p.Id == identityValidation.ProfileId);
                    profile.IdentityValidated = true;
                    _unitOfWork.ProfileRepository.Update(profile);

                }
            }

            _unitOfWork.IdentityValidationRepository.Update(identityValidation);
            await _unitOfWork.Save();
            //DBContext.Entry(identityValidation).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return identityValidation;

        }

        public async Task<object> RemoveImagesIdentityValidation(Guid idIdentityValidation)
        {
            // validate id
            if (idIdentityValidation.ToString().Length < 6) return new { error = "Solicitud de validación no válida" };
            IdentityValidation identityValidation = _unitOfWork.IdentityValidationRepository
                                    .FirstOrDefault(p => p.Id == idIdentityValidation);
            if (identityValidation == null)
                return new { error = "La solicitud de validación de identidad no coincide con ninguna de la base de datos" };

            //delete  IdentityValidationImages
            List<IdentityValidationImage> identityValidationImages = _unitOfWork.IdentityValidationImageRepository
                                    .GetAll(i => i.IdentityValidationId == idIdentityValidation).ToList();
            foreach (IdentityValidationImage p in identityValidationImages)
            {
                p.Deleted = true;
            }

            _unitOfWork.IdentityValidationImageRepository.Update(identityValidationImages);

            await _unitOfWork.Save();
            return true;
        }
        public async Task<object> AddImageToIdentityValidation(Guid idImage, Guid idIdentityValidation, string? type)
        {
            // validate id
            if (idImage.ToString().Length < 6) return new { error = "Ingrese una imagen válida" };
            if (idIdentityValidation.ToString().Length < 6) return new { error = "Solicitud no válida" };
            Image image = _unitOfWork.ImageRepository.FirstOrDefault(i => i.Id == idImage);
            if (image == null) return new { error = "La imagen no coincide con ninguna imagen de la base de datos" };
            IdentityValidation identityValidation = _unitOfWork.IdentityValidationRepository.FirstOrDefault(p => p.Id == idIdentityValidation);
            if (identityValidation == null) return new { error = "La solicitud de validación de identidad no coincide con ninguna de la base de datos" };

            //create  IdentityValidationImage
            IdentityValidationImage identityValidationImage;
            IdentityValidationImage repeated = _unitOfWork.IdentityValidationImageRepository.FirstOrDefault(p =>
                                                                    p.ImageId == idImage
                                                                    && p.IdentityValidationId == idIdentityValidation);
            if (repeated == null)
            {
                identityValidationImage = new IdentityValidationImage();
                identityValidationImage.Id = Guid.NewGuid();
                identityValidationImage.Deleted = false;
                identityValidationImage.ImageId = idImage;
                identityValidationImage.IdentityValidationId = idIdentityValidation;
                identityValidationImage.Type = type;

                _unitOfWork.IdentityValidationImageRepository.Insert(identityValidationImage);
            }
            else
            {
                identityValidationImage = repeated;
                identityValidationImage.Type = type;
                identityValidationImage.Deleted = false;


            }
            await _unitOfWork.Save();
            return _unitOfWork.IdentityValidationImageRepository.FirstOrDefault(p => p.Id == identityValidationImage.Id);
        }

        public async Task<object> HasPendingIdentityValidation(Guid profileId)
        {
            Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == profileId);
            if (profile == null)
                return new { error = "El perfil no coincide con ningún perfil de la base de datos" };

            IdentityValidation identityValidation = _unitOfWork.IdentityValidationRepository
                                    .FirstOrDefault(p => p.ProfileId == profileId &&
                                    (p.StatusId == (int)Enums.State.Pendiente_IdentityValidation
                                   || p.StatusId == (int)Enums.State.Aprobada_IdentityValidation));
            if (identityValidation == null) 
                return false;

            return true;
        }
        public async Task<object> Delete(Guid id)
        {

            IdentityValidation identityValidation = _unitOfWork.IdentityValidationRepository.FirstOrDefault(u => u.Id == id);
            if (identityValidation == null) return new { error = "El id ingresado no es válido" };
            identityValidation.Deleted = true;
            _unitOfWork.IdentityValidationRepository.Update(identityValidation);
            await _unitOfWork.Save();
            //DBContext.Entry(identityValidation).State = EntityState.Modified;
            //DBContext.SaveChanges();
            return identityValidation;
        }



        #endregion

    }
}
