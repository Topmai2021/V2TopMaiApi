using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Profiles.Interfaces;

namespace TopMai.Domain.Services.Profiles
{
    public class ProfileReviewService : IProfileReviewService
    {
        private DataContext DBContext;
        private readonly IUnitOfWork _unitOfWork;

        #region Builder
        public ProfileReviewService(DataContext dBContext, IUnitOfWork unitOfWork)
        {
            DBContext = dBContext;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public List<ProfileReview> GetAll()
        {
            var profileReviews = DBContext.ProfileReviews.OrderByDescending(x => x.Id).ToList();
            foreach (var profileReview in profileReviews)
            {
                profileReview.ReviewType = DBContext.ReviewTypes.FirstOrDefault(x => x.Id == profileReview.ReviewTypeId);
            }

            return profileReviews;
        }

        public object Get(Guid? id)
        {
            var profileReview = DBContext.ProfileReviews.FirstOrDefault(u => u.Id == id);
            if (profileReview == null)
                return new { value = "Perfil no encontrado" };
            if (profileReview.ReviewTypeId != null)
                profileReview.ReviewType = DBContext.ReviewTypes.FirstOrDefault(u => u.Id == profileReview.ReviewTypeId);

            return profileReview;
        }

        public object Post(ProfileReview profileReview)
        {
            if (profileReview.FromId == null || profileReview.FromId.ToString().Length < 6)
                return new { error = "El id del emisor de la reseña debe ser al menos 6 caracteres" };

            if (profileReview.ToId == null || profileReview.ToId.ToString().Length < 6)
                return new { error = "El id del receptor de la reseña debe ser al menos 6 caracteres" };
            if (profileReview.SellId == null || profileReview.SellId.ToString().Length < 6)
                return new { error = "El id de la venta debe ser al menos 6 caracteres" };

            if (profileReview.FromId == profileReview.ToId)
                return new { error = "No se puede hacer una reseña a si mismo" };
            if (profileReview.ReviewTypeId == null)
                return new { error = "Debe seleccionar un tipo de reseña" };

            Sell sell = DBContext.Sells.FirstOrDefault(x => x.Id == profileReview.SellId);
            if (sell == null)
                return new { error = "La venta no existe" };

            ProfileReview repeatedProfileReview = DBContext.ProfileReviews
                    .FirstOrDefault(x => x.FromId == profileReview.FromId
                                     && x.ToId == profileReview.ToId
                                    && x.ReviewTypeId == profileReview.ReviewTypeId
                                    && x.SellId == profileReview.SellId);

            if (repeatedProfileReview != null)
            {
                repeatedProfileReview.Valoration = profileReview.Valoration;
                DBContext.Update(repeatedProfileReview);
                DBContext.SaveChanges();
                return repeatedProfileReview;

            }

            Profile from = DBContext.Profiles.FirstOrDefault(u => u.Id == profileReview.FromId);
            Profile to = DBContext.Profiles.FirstOrDefault(u => u.Id == profileReview.ToId);
            if (from == null)
                return new { error = "El emisor de la reseña no existe o no tiene el perfil completado" };

            if (to == null)
                return new { error = "El receptor de la reseña no existe o no tiene el perfil completado" };

            if (profileReview.Valoration < 1 || profileReview.Valoration > 5)
                return new { error = "La valoración debe estar entre 1 y 5" };

            profileReview.Id = Guid.NewGuid();
            profileReview.DateTime = DateTime.Now;

            DBContext.ProfileReviews.Add(profileReview);
            DBContext.SaveChanges();
            ProfileReview? res = DBContext.ProfileReviews.Where(p => p.Id == profileReview.Id).FirstOrDefault();

            return (res);
        }



        public object Put(ProfileReview newProfileReview)
        {
            var idProfileReview = newProfileReview.Id;
            if (idProfileReview == null || idProfileReview.ToString().Length < 6)
                return new { error = "Ingrese un id de reseña válido " };

            ProfileReview? profileReview = DBContext.ProfileReviews.Where(c => c.Id == idProfileReview && newProfileReview.Id != null).FirstOrDefault();
            if (profileReview == null)
                return new { error = "El id no coincide con ninguna reseña" };
            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newProfileReview.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newProfileReview) != null && propertyInfo.GetValue(newProfileReview).ToString() != "")
                    propertyInfo.SetValue(profileReview, propertyInfo.GetValue(newProfileReview));

            }

            DBContext.Entry(profileReview).State = EntityState.Modified;
            DBContext.SaveChanges();

            return profileReview;
        }

        public object IsCalificated(Guid? fromId, Guid? toId, Guid? sellId)
        {
            if (fromId == null || fromId.ToString().Length < 6)
                return new { error = "El id del emisor de la reseña debe ser al menos 6 caracteres" };

            if (toId == null || toId.ToString().Length < 6)
                return new { error = "El id del receptor de la reseña debe ser al menos 6 caracteres" };
            if (sellId == null || sellId.ToString().Length < 6)
                return new { error = "El id de la venta debe ser al menos 6 caracteres" };

            ProfileReview repeatedProfileReview = DBContext.ProfileReviews
                    .FirstOrDefault(x => x.FromId == fromId
                                     && x.ToId == toId
                                    && x.SellId == sellId);

            if (repeatedProfileReview != null)
            {
                return true;
            }
            return false;
        }

        public object GetProfileReviewsByProfileId(Guid? id)
        {
            if (id == null || id.ToString().Length < 6)
                return new { error = "Ingrese un id de perfil válido " };

            Profile profile = DBContext.Profiles.FirstOrDefault(u => u.Id == id);
            if (profile == null)
                return new { error = "El id no coincide con ningún perfil" };

            var profileReviews = DBContext.ProfileReviews.Where(u => u.ToId == id).ToList();
            foreach (var profileReview in profileReviews)
            {
                profileReview.From = DBContext.Profiles.FirstOrDefault(u => u.Id == profileReview.FromId);
                profileReview.ReviewType = DBContext.ReviewTypes.FirstOrDefault(x => x.Id == profileReview.ReviewTypeId);
            }

            return profileReviews;
        }

        public object GetMyValorationToProfileId(Guid id, Guid toId)
        {
            if (id == null || id.ToString().Length < 6)
                return new { error = "Ingrese un id de perfil válido " };
            Profile profile = DBContext.Profiles.FirstOrDefault(u => u.Id == id);

            if (profile == null)
                return new { error = "El id no coincide con ningún perfil" };

            var profileReviews = DBContext.ProfileReviews.Where(u => u.FromId == id && u.ToId == toId).ToList();
            if (profileReviews.Count == 0)
                return new { error = "No hay reseñas para este perfil" };

            List<ReviewType> reviewTypes = DBContext.ReviewTypes.ToList();

            List<object> valorationList = new List<object>();
            foreach (var reviewType in reviewTypes)
            {
                foreach (var profileReview in profileReviews)
                {
                    if (profileReview.ReviewTypeId == reviewType.Id)
                    {
                        var valorationObject = new { reviewType.Name, valoration = profileReview.Valoration };
                        valorationList.Add(valorationObject);
                    }
                }

            }

            return valorationList;
        }

        public object GetValorationByProfileId(Guid? id)
        {
            if (id == null || id.ToString().Length < 6)
                return new { error = "Ingrese un id de perfil válido " };

            Profile profile = DBContext.Profiles.FirstOrDefault(u => u.Id == id);
            if (profile == null)
                return new { error = "El id no coincide con ningún perfil" };

            var profileReviews = DBContext.ProfileReviews.Where(u => u.ToId == id).ToList();
            if (profileReviews.Count == 0)
                return new { error = "El perfil no tiene reseñas" };

            List<ReviewType> reviewTypes = DBContext.ReviewTypes.ToList();
            List<object> valorationList = new List<object>();

            //global
            float valoration = 0;
            foreach (var profileReview in profileReviews)
            {
                valoration += (int)profileReview.Valoration;
            }
            valoration = (float)valoration / profileReviews.Count;

            object o = new object();
            o = new { name = "Global", valoration = valoration };
            valorationList.Add(o);


            foreach (var reviewType in reviewTypes)
            {
                valoration = 0;
                var reviews = profileReviews.Where(x => x.ReviewTypeId == reviewType.Id).ToList();
                foreach (var profileReview in reviews)
                {
                    if (profileReview.ReviewTypeId == reviewType.Id)
                    {
                        valoration += (int)profileReview.Valoration;
                    }
                }
                if (reviews.Count == 0)
                {
                    valoration = 0;
                }
                else
                {
                    valoration = (float)valoration / reviews.Count;

                }
                var name = reviewType.Name;
                object obj = new object();
                obj = new { name = name, valoration = valoration };
                valorationList.Add(obj);


            }

            return valorationList;
        }

        public async Task<bool> Delete(Guid idProfileReview)
        {
            ProfileReview profileReview = _unitOfWork.ProfileReviewRepository.FirstOrDefault(u => u.Id == idProfileReview);
            if (profileReview == null)
                throw new BusinessException("No se encontró la reseña");

            _unitOfWork.ProfileReviewRepository.Delete(profileReview);

            return await _unitOfWork.Save() > 0;
        }
        #endregion
    }
}
