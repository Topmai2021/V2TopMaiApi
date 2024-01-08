using Common.Utils.Enums;
using Common.Utils.Exceptions;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.DTOs;
using Infraestructure.Entity.Entities.Payments;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using Infraestructure.Entity.Request;
using Microsoft.EntityFrameworkCore;
using TopMai.Domain.DTO.Products;
using TopMai.Domain.Services.Payments.Interfaces;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class PublicationService : IPublicationService
    {

        #region Attributes
        private DataContext _dataContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrencyService _currencyService;
        private readonly IPublicationCommentService _publicationComment;
        #endregion


        #region Builder
        public PublicationService(DataContext dataContext, IUnitOfWork unitOfWork, ICurrencyService currencyService, IPublicationCommentService publicationComment)
        {
            _dataContext = dataContext;
            _unitOfWork = unitOfWork;
            _currencyService = currencyService;
        }
        #endregion

        #region Methods


        public async Task<PublicationResult> GetAll(int pageNumber, int pageSize)
        {
            List<Publication> publications = new List<Publication>();
            try
            {
                long TotalRecords = _dataContext.Publications.Count();
                int totalPages = (int)Math.Ceiling((double)TotalRecords / pageSize);

                // Ensure pageNumber is within valid range
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }
                else if (pageNumber > totalPages)
                {
                    pageNumber = totalPages;
                }

                // Calculate the number of records to skip and take
                int skipAmount = (pageNumber - 1) * pageSize;
                publications = await _dataContext.Publications.OrderByDescending(x => x.PublicationDate).Skip(skipAmount).Take(pageSize).ToListAsync();

                foreach (var publication in publications)
                {
                    publication.Publisher = await _dataContext.Profiles.Where(x => x.Id == publication.PublisherId).FirstOrDefaultAsync();
                    // publication.Subcategory = await _dataContext.Subcategories.Where(x => x.Id == publication.SubcategoryId).FirstOrDefaultAsync();
                    // publication.Subcategory.Category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == publication.Subcategory.CategoryId);
                    // publication.Currency = await _dataContext.Currencys.FirstOrDefaultAsync(x => x.Id == publication.CurrencyId);
                }

                List<Publication> publicationsWithoutNumber = publications.Where(x => x.Number == null).ToList();

                if (publicationsWithoutNumber != null && publicationsWithoutNumber.Any())
                {
                    foreach (Publication publication in publicationsWithoutNumber)
                    {
                        if (publications.Max(x => x.Number) == null)
                        {
                            publication.Number = 1;
                        }
                        else
                        {
                            publication.Number = publications.Max(x => x.Number) + 1;
                        }

                        _unitOfWork.PublicationRepository.Update(publication);
                    }

                    await _unitOfWork.Save();
                }

                var result = new PublicationResult
                {
                    totalPages = totalPages,
                    pageNumber = pageNumber,
                    TotalRecords = TotalRecords,
                    publications = publications
                };

                return result;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return new PublicationResult();
            }
        }
        /*public async Task<List<Publication>> GetAll(int pageNumber, int pageSize)
        {
            List<Publication> publications = new List<Publication>();
            try
            {
                long TotalRecords = _dataContext.Publications.Count();
                int totalPages = (int)Math.Ceiling((double)TotalRecords / pageSize);
                // Ensure pageNumber is within valid range
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }
                else if (pageNumber > totalPages)
                {
                    pageNumber = totalPages;
                }
                // Calculate the number of records to skip and take
                int skipAmount = (pageNumber - 1) * pageSize;
                publications = await _dataContext.Publications.OrderByDescending(x => x.PublicationDate).Skip(skipAmount).Take(pageSize).ToListAsync();
                foreach (var publication in publications)
                {
                    publication.TotalPage = totalPages;
                    publication.PageNumber = pageNumber;
                    publication.TotalCount = TotalRecords;
                    publication.Publisher = await _dataContext.Profiles.Where(x => x.Id == publication.PublisherId).FirstOrDefaultAsync();
                    publication.Subcategory = await _dataContext.Subcategories.Where(x => x.Id == publication.SubcategoryId).FirstOrDefaultAsync();
                    publication.Subcategory.Category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == publication.Subcategory.CategoryId);
                    publication.Currency = await _dataContext.Currencys.FirstOrDefaultAsync(x => x.Id == publication.CurrencyId);
                }
                //.Include("Publisher")
                //.Include("Subcategory")
                //.Include("Subcategory.Category")
                //.Include("Currency")

                List<Publication> publicationsWithoutNumber = publications.Where(x => x.Number == null).ToList();

                if (publicationsWithoutNumber != null && publicationsWithoutNumber.Any())
                {
                    foreach (Publication publication in publicationsWithoutNumber)
                    {

                        if (publications.Max(x => x.Number) == null)
                        {
                            publication.Number = 1;
                        }
                        else
                        {
                            publication.Number = publications.Max(x => x.Number) + 1;
                        }

                        _unitOfWork.PublicationRepository.Update(publication);
                    }

                    await _unitOfWork.Save();
                }

                return publications;
            }
            catch (Exception ex)
            {
                return publications;
            }



        }*/
        public Currency? GetCurrency(Publication publication)
        {
            return _dataContext.Currencys.FirstOrDefault(c => c.Id == publication.CurrencyId);
        }
        private Publication Single(Guid id) => _unitOfWork.PublicationRepository.FirstOrDefault(x => x.Id == id);

        public async Task<Publication> Get(Guid id)
        {
            var publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == id);

            if (publication != null)
            {
                publication.PublicationImages = (List<Infraestructure.Entity.Entities.Profiles.Image>)GetImagesByPublication((Guid)publication.Id);
                publication.Currency = GetCurrency(publication);
                publication.Publisher =  _dataContext.Profiles.Where(x => x.Id == publication.PublisherId).FirstOrDefault();
                publication.Subcategory = _unitOfWork.SubcategoryRepository.FirstOrDefault(s => s.Id == publication.SubcategoryId);
                publication.Category = GetCategory(publication);

            }

            publication.PublicationComments = _unitOfWork.PublicationCommentRepository.GetAll().Where(x => x.PublicationId == publication.Id).ToList();
              
                foreach (var comment in publication.PublicationComments)
                {
                    comment.From = _dataContext.Profiles.Where(x => x.Id == comment.FromId).FirstOrDefault();
                 
                }

            if (publication?.UrlPrincipalImage == null && publication.PublicationImages.Count > 0)
                publication.UrlPrincipalImage = publication.PublicationImages?.FirstOrDefault().UrlImage;

            publication.Currency = GetCurrency(publication);

            await AddNewVisit(publication);
            CountComents(publication);
            return publication;
        }

        private void CountComents(Publication publication)
        {
            if (publication != null)
                publication.CommentsCount = publication.PublicationComments.Count();

            _unitOfWork.Save();
        }


        public List<ConsultPublication_Dto> GetHomePublications()
        {
            var result = _dataContext.Publications
                .Where(p => !p.Deleted && p.Status == 1)
                .Include("Currency")
                .OrderByDescending(p => p.Visits)
                .Take(10)
                .ToList();

            var listPublicatios = result.Select(x => new ConsultPublication_Dto
            {
                Id = x.Id,
                Price = x.Price,
                StrCurrency = x.Currency.Abbreviation,
                UrlPrincipalImage = x.UrlPrincipalImage
            }).ToList();

            return listPublicatios;
        }

        public object GetActivePublications(int? page)
        {
            if (page == 0)
            {
                IEnumerable<Publication> publications = _unitOfWork.PublicationRepository.FindAll(x => x.Deleted != true &&
                                                                                            x.Status == 1,
                                                                                            c => c.Currency).ToList();

                return publications.OrderByDescending(x => x.PublicationDate).Take(6);
            }

            if (page >= 2)
            {
                List<Publication> publications = _unitOfWork.PublicationRepository.FindAll(x => x.Deleted != true
                                                                                             && x.Status == 1,
                                                                                            c => c.Currency).ToList();
                return publications.OrderByDescending(p => p.PublicationDate).ToList().Skip(6 * ((int)page - 1)).Take(6);
            }
            else
            {
                List<Publication> publications = _unitOfWork.PublicationRepository.FindAll(x => x.Deleted != true
                                                                                                            && x.Status == 1,
                                                                                                           c => c.Currency).ToList();
                return publications.OrderByDescending(p => p.PublicationDate);

            }
        }

        public object Post(PublicationDTO publication)
        {
            Profile? profile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == publication.PublisherId);
            if (profile == null)
                return new { error = "El usuario debe tener el perfil completo para publicar" };

            if (profile.Land != null && profile.Land.ToLower() == "mexico")
            {
                return new { error = "Solo se permiten publicaciones de México" };
            }
            if (publication.Name == null || publication.Name.Length < 3)
                return new { error = "El titulo de la publicación debe ser al menos 3 caracteres" };
            if (publication.SubcategoryId == null)
                return new { error = "Seleccione una subcategoria válida" };

            if (publication.Description == null || publication.Description.Length < 3)
                return new { error = "La descripción debe ser al menos 3 caracteres" };
            Subcategory? subcategory = _unitOfWork.SubcategoryRepository.FirstOrDefault(s => s.Id == publication.SubcategoryId);
            if (subcategory == null)
                return new { error = "Seleccione una subcategoria válida" };

            Publication newPublication = new Publication()
            {
                Id = publication.Id,
                Name = publication.Name,
                HasInternationalShipping = publication.HasInternationalShipping,
                PublisherId = publication.PublisherId,
                Deleted = publication.Deleted,
                CurrencyId = publication.CurrencyId,
                HasPersonalDelivery = publication.HasPersonalDelivery,
                HasFreeShippment = publication.HasFreeShippment,
                HasPickup = publication.HasPickup,
                SubcategoryId = publication.SubcategoryId,
                Weight = publication.Weight,
                ShippmentPrice = publication.ShippmentPrice,
                Condition = publication.Condition,
                ReceiveOffers = publication.ReceiveOffers,
                Price = publication.Price,
                Description = publication.Description,
                PublicationDate = publication.PublicationDate,
                Status = publication.Ambit
            };


            if (publication.HasInternationalShipping == null)
                publication.HasInternationalShipping = false;

            if (publication.ReceiveOffers == null)
                publication.ReceiveOffers = true;


            if (publication.CurrencyId == 0)
                publication.CurrencyId = (int)Enums.Currency.MXN;
            else
            {
                Currency? currency = _unitOfWork.CurrencyRepository.FirstOrDefault(c => c.Id == publication.CurrencyId);
                if (currency == null)
                    return new { error = "Selecciona una divisa válida" };
            }


            _unitOfWork.PublicationRepository.Insert(newPublication);
            _unitOfWork.SaveChanges();

            return newPublication.Id;
        }

        public object Put(PublicationUpdateRequest newPublication)
        {
            var idPublication = newPublication.Id;

            if (newPublication.Price != null)
            {
                if (newPublication.Price < 1)
                {
                    return new { error = "El precio debe ser mayor a 0" };
                }
            }

            if (idPublication == null || idPublication.ToString().Length < 6)
                return new { error = "Seleccione una publicación válida " };

            Publication? publication = _unitOfWork.PublicationRepository.FirstOrDefault(c => c.Id == idPublication && newPublication.Id != null);

            if (publication == null)
                return new { error = "Seleccione una publicación válida" };

            else if (newPublication.Price > publication.Price)
            {
                publication.PriceNote = null;
            }

            //loop through each attribute entered and modify it

            publication.Name = newPublication.Name == null ? publication.Name : newPublication.Name;
            publication.Description = newPublication.Description == null ? publication.Description : newPublication.Description;
            publication.UrlPrincipalImage = newPublication.UrlPrincipalImage == null ? publication.UrlPrincipalImage : newPublication.UrlPrincipalImage;
            publication.PublisherId = newPublication.PublisherId;


            _unitOfWork.PublicationRepository.Update(publication);
            _unitOfWork.SaveChanges();

            return publication;
        }

        public object RemoveMultiplePublications(List<Guid> ids)
        {
            List<Publication> publications = _unitOfWork.PublicationRepository.GetAll().Where(p => ids.Contains((Guid)p.Id)).ToList();
            if (publications.Count == 0)
                return new { error = "Seleccione al menos una publicación" };

            foreach (Publication publication in publications)
            {
                publication.Deleted = true;
                publication.EndDate = DateTime.Now;
                publication.DeletedDate = DateTime.Now;
            }

            _unitOfWork.Save();

            return publications;
        }

        public object RenewMultiplePublications(List<Guid> ids)
        {
            List<Publication> publications = _unitOfWork.PublicationRepository.GetAll().Where(p => ids.Contains((Guid)p.Id)).ToList();
            if (publications.Count == 0)
                return new { error = "Seleccione al menos una publicación" };

            foreach (Publication publication in publications)
            {
                publication.Deleted = false;
                publication.DeletedDate = null;
                publication.EndDate = null;
            }

            _unitOfWork.Save();

            return publications;
        }

        public object Delete(Guid id)
        {

            Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == id);

            if (publication == null)
            {
                return publication;
            }

            if(publication.Deleted==true){
             _unitOfWork.PublicationRepository.Delete(publication);
            }else{
             publication.Deleted = true;
              _unitOfWork.PublicationRepository.Update(publication);
            }
       _unitOfWork.SaveChanges();
           
            return publication;
        }

        public object GetPublicationsBySubcategory(Guid id)
        {
            var publications = _unitOfWork.PublicationRepository.GetAll()
                            .Where(p => p.SubcategoryId == id && p.Deleted != true)
                            .OrderByDescending(p => p.PublicationDate).ToList();

            foreach (var pub in publications)
            {
                pub.Currency = _currencyService.Get(pub.CurrencyId);
            }

            return publications;
        }

        public Category? GetCategory(Publication publication)
        {
            Subcategory sub = _unitOfWork.SubcategoryRepository.FirstOrDefault(x => x.Deleted != true
                                                                                 && x.Id == publication.SubcategoryId
                                                                                 && x.CategoryId != null,
                                                                                c => c.Category);

            if (sub != null)
                return sub.Category;


            return null;
        }
        public object GetPublicationsByCategory(MessageRequest request)
        {
            if (request.page == 0)
            {

                List<Publication> publications = _unitOfWork.PublicationRepository
                    .FindAll(x => x.Subcategory.CategoryId == request.id && x.Deleted != true && x.Status == 1, includeProperties: u => u.Subcategory).ToList();


                return publications.OrderByDescending(p => p.PublicationDate).Take(6);
            }

            if (request.page >= 2)
            {
                List<Publication> publications = _unitOfWork.PublicationRepository
                    .FindAll(x => x.Subcategory.CategoryId == request.id && !x.Deleted && x.Status == 1, includeProperties: u => u.Subcategory).ToList();

                return publications.OrderByDescending(p => p.PublicationDate).ToList().Skip(6 * ((int)request.page - 1)).Take(6);
            }
            else
            {

                List<Publication> publications = _unitOfWork.PublicationRepository
                    .FindAll(x => x.Subcategory.CategoryId == request.id && !x.Deleted && x.Status == 1, includeProperties: u => u.Subcategory).ToList();

                return publications.OrderByDescending(p => p.PublicationDate);
            }
        }

        public object GetPublicationsByProfile(Guid id)
        {
            var publications = _unitOfWork.PublicationRepository.GetAll().Where(p => p.PublisherId == id && p.Deleted != true).ToList();
            foreach (Publication publication in publications)
            {
                publication.PublicationImages = (List<Image>)GetImagesByPublication((Guid)publication.Id);
                publication.Currency = _currencyService.Get(publication.CurrencyId);
                publication.Category = GetCategory(publication);
                publication.Subcategory = null;
            }

            return publications.OrderByDescending(p => p.PublicationDate);
        }


        public List<Image> GetImagesByPublication(Guid id)
        {
            var publicationImages = _unitOfWork.PublicationImageRepository.FindAll(p => p.PublicationId == id
                                                                                  && p.Deleted != true,
                                                                                i => i.Image).ToList();
            List<Image> images = publicationImages.Select(x => x.Image).ToList();

            return images;
        }

        public object AddShippmentTypeToPublication(int idShippmentType, Guid idPublication, float? price)
        {
            ShippmentType shippmentType = _unitOfWork.ShippmentTypeRepository.FirstOrDefault(s => s.Id == idShippmentType);
            if (shippmentType == null)
                return new { error = "Seleccione un tipo de envio" };

            Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == idPublication);
            if (publication == null)
                return new { error = "Seleccione una publicación" };

            PublicationShippmentType publicationShippmentType = new PublicationShippmentType();
            publicationShippmentType.Id = Guid.NewGuid();
            publicationShippmentType.PublicationId = idPublication;
            publicationShippmentType.ShippmentTypeId = idShippmentType;
            publicationShippmentType.Price = price;
            _unitOfWork.PublicationShippmentTypeRepository.Insert(publicationShippmentType);
            _unitOfWork.Save();

            return publicationShippmentType.Id;
        }

        public object GetShippmentTypeByPublication(Guid id)
        {
            var publicationShippmentTypes = _unitOfWork.PublicationShippmentTypeRepository.GetAll().Where(p => p.PublicationId == id).ToList();
            if (publicationShippmentTypes == null)
                return null;

            return publicationShippmentTypes;
        }

        public Task<Guid> AddNewVisit(Publication publication)
        {

            if (publication == null)
                throw new BusinessException("Seleccione una publicación");

            if (publication.Visits == null)
                publication.Visits = 1;
            else
                publication.Visits += 1;


            _unitOfWork.PublicationRepository.Update(publication);
            _unitOfWork.Save();

            return Task.FromResult(publication.Id);
        }

        public List<Publication> DefaultSearch(string query)
        {
            //return DBContext.Publications
            //    .Include("Currency").Include("Publisher").Include("Publisher.Image")
            //    .Where(p => (p.Name.Contains(query) || p.Description.Contains(query)) && p.Deleted != true)
            //    .OrderByDescending(p => p.PublicationDate).Take(100).ToList();

            return _dataContext.Publications
               .Include("Currency")
               .Where(p => (p.Name.Contains(query) || p.Description.Contains(query)) && p.Deleted != true)
               .OrderByDescending(p => p.PublicationDate).Take(100).ToList();
        }

        public object SearchPublication(string query, int filter)
        {
            switch (filter)
            {
                case 0: return DefaultSearch(query);
                case 1:
                    //return DBContext.Publications
                    //        .Include("Currency").Include("Publisher").Include("Publisher.Image")
                    //        .Where(p => (p.Name.Contains(query) || p.Description.Contains(query)) && p.Deleted != true)
                    //        .OrderBy(p => p.Price).Take(100).ToList();
                    return _dataContext.Publications
                          .Include("Currency")
                          .Where(p => (p.Name.Contains(query) || p.Description.Contains(query)) && p.Deleted != true)
                          .OrderBy(p => p.Price).Take(100).ToList();
                case 2:
                    //return DBContext.Publications
                    //        .Include("Currency").Include("Publisher").Include("Publisher.Image")
                    //        .Where(p => (p.Name.Contains(query) || p.Description.Contains(query)) && p.Deleted != true)
                    //        .OrderByDescending(p => p.Price).Take(100).ToList();

                    return _dataContext.Publications.Include("Currency")
                           .Where(p => (p.Name.Contains(query) || p.Description.Contains(query)) && p.Deleted != true)
                           .OrderByDescending(p => p.Price).Take(100).ToList();
                default: return DefaultSearch(query);
            }
        }

        public object AddImageToPublication(Guid idImage, Guid idPublication, int? number)
        {
            // validate id
            if (idImage.ToString().Length < 6) return new { error = "Ingrese una imagen válida" };
            if (idPublication.ToString().Length < 6) return new { error = "Publicación no válida" };
            Image? image = _unitOfWork.ImageRepository.FirstOrDefault(i => i.Id == idImage);
            if (image == null) return new { error = "La imagen no coincide con ninguna imagen de la base de datos" };
            Publication? publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == idPublication);
            if (publication == null) return new { error = "La publicacion no coincide con ninguna de la base de datos" };

            //create  PublicationImage
            PublicationImage publicationImage;
            PublicationImage repeated = _unitOfWork.PublicationImageRepository.FirstOrDefault(p =>
                                                                    p.ImageId == idImage
                                                                    && p.PublicationId == idPublication);
            if (repeated == null)
            {
                publicationImage = new PublicationImage();
                publicationImage.Id = Guid.NewGuid();
                publicationImage.Deleted = false;
                publicationImage.ImageId = idImage;
                publicationImage.PublicationId = idPublication;
                publicationImage.Number = number;

                _unitOfWork.PublicationImageRepository.Insert(publicationImage);
            }
            else
            {
                publicationImage = repeated;
                publicationImage.Number = number;
                publicationImage.Deleted = false;


            }
            _unitOfWork.Save();
            return publicationImage.Id;
        }

        public object RemoveImagePublications(Guid idPublication)
        {
            // validate id
            if (idPublication.ToString().Length < 6) return new { error = "Publicación no válida" };
            Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == idPublication);
            if (publication == null) return new { error = "La publicacion no coincide con ninguna de la base de datos" };

            //create  PublicationImage
            List<PublicationImage> publicationImages = _unitOfWork.PublicationImageRepository.GetAll().Where(p => p.PublicationId == idPublication).ToList();
            foreach (PublicationImage p in publicationImages)
            {
                p.Deleted = true;
            }

            _unitOfWork.Save();
            return true;
        }

        public async Task<Guid> AddNewVisitEndPoint(Guid id)
        {
            Publication publication = Single(id);

            if (publication == null)
                throw new BusinessException("Seleccione una publicación");

            if (publication.Visits == null)
                publication.Visits = 1;
            else
                publication.Visits += 1;


            _unitOfWork.PublicationRepository.Update(publication);
            await _unitOfWork.Save();

            return publication.Id;
        }

        #endregion
    }
}
