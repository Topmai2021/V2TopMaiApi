using Infraestructure.Core.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Products.Interfaces;
using PublicationLike = Infraestructure.Entity.Entities.Products.PublicationLike;
using Attribute = Infraestructure.Entity.Entities.Products.Attribute;
using Infraestructure.Entity.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Core.Data;

namespace TopMai.Domain.Services.Products
{
    public class PublicationLikeService : IPublicationLikeService
    {

        #region Attributes
        private IUnitOfWork _unitOfWork;
        private DataContext _dbContext;
        #endregion

        #region Builder
        public PublicationLikeService(IUnitOfWork unitOfWork,DataContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;

        }
        #endregion

        #region Methods
        public List<PublicationLike> GetAll() => _unitOfWork.PublicationLikeRepository.GetAll().ToList();



        public PublicationLike Get(Guid id) => _unitOfWork.PublicationLikeRepository.FirstOrDefault(u => u.Id == id);


        public async Task<object> Post (PublicationLike publicationLike)
        {
            Profile profile = _dbContext.Profiles.Where(p => p.Id == publicationLike.FromId).FirstOrDefault();
            if (profile == null) return new { error = "Debe completar su perfil" };
            Publication publication = _dbContext.Publications.Where(p => p.Id == publicationLike.PublicationId).FirstOrDefault();
            if (publication == null) return new { error = "La publicación no es válida" };

            PublicationLike repeated = _dbContext.PublicationLikes.Where(p => p.PublicationId == publicationLike.PublicationId && p.FromId == publicationLike.FromId).FirstOrDefault();
            if (repeated != null)
            {
                repeated.Deleted = !repeated.Deleted;
                _dbContext.Entry(repeated).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return repeated;


            }
            else
            {
                publicationLike.Id = Guid.NewGuid();
                publicationLike.Deleted = false;

                _dbContext.PublicationLikes.Add(publicationLike);
                _dbContext.SaveChanges();
            }

            return _dbContext.PublicationLikes.Where(p => p.Id == publicationLike.Id).FirstOrDefault();


        }


            public async Task<object> Put(PublicationLike newPublicationLike)
            {
                var idPublicationLike = newPublicationLike.Id;
                if (idPublicationLike == null || idPublicationLike.ToString().Length < 6) return new { error = "Ingrese un id de rol válido " };


                PublicationLike? PublicationLike = _unitOfWork.PublicationLikeRepository.FirstOrDefault(v => v.Id == idPublicationLike && newPublicationLike.Id != null);
                //PublicationLike? PublicationLike = _dbContext.PublicationLikes.Where(r => r.Id == idPublicationLike && newPublicationLike.Id != null).FirstOrDefault();
                if (PublicationLike == null) return new { error = "El id no coincide con ningun rol " };

                //loop through each attribute entered and modify it

                foreach (PropertyInfo propertyInfo in newPublicationLike.GetType().GetProperties())
                {
                    if (propertyInfo.GetValue(newPublicationLike) != null && propertyInfo.GetValue(newPublicationLike).ToString() != "")
                    {
                        propertyInfo.SetValue(PublicationLike, propertyInfo.GetValue(newPublicationLike));

                    }

                }

                _unitOfWork.PublicationLikeRepository.Update(PublicationLike);
                await _unitOfWork.Save();
                //_dbContext.Entry(PublicationLike).State = EntityState.Modified;
                //_dbContext.SaveChanges();
                return PublicationLike;

            }

            public async Task<object> Delete(Guid id)
            {

                PublicationLike PublicationLike = _unitOfWork.PublicationLikeRepository.FirstOrDefault(u => u.Id == id);
                if (PublicationLike == null) return new { error = "El id ingresado no es válido" };
                PublicationLike.Deleted = true;
                _unitOfWork.PublicationLikeRepository.Update(PublicationLike);
                await _unitOfWork.Save();
                //_dbContext.Entry(PublicationLike).State = EntityState.Modified;
                //_dbContext.SaveChanges();
                return PublicationLike;
            }

        }
        #endregion

    }
