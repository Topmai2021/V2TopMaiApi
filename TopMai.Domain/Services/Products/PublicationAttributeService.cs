using Infraestructure.Core.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TopMai.Domain.Services.Products.Interfaces;
using PublicationAttribute = Infraestructure.Entity.Entities.Products.PublicationAttribute;
using Attribute = Infraestructure.Entity.Entities.Products.Attribute;
using Infraestructure.Entity.Entities.Products;

namespace TopMai.Domain.Services.Products
{
            public class PublicationAttributeService : IPublicationAttributeService
            {

                        #region Attributes
                        private IUnitOfWork _unitOfWork;
                        #endregion

                        #region Builder
                        public PublicationAttributeService(IUnitOfWork unitOfWork)
                        {
                                    _unitOfWork = unitOfWork;

                        }
                        #endregion

                        #region Methods
                        public List<PublicationAttribute> GetAll() => _unitOfWork.PublicationAttributeRepository.GetAll().ToList();



                        public PublicationAttribute Get(Guid id) => _unitOfWork.PublicationAttributeRepository.FirstOrDefault(u => u.Id == id);


                        public async Task<object> Post(PublicationAttribute publicationAttribute)
                        {

                                    Attribute attribute = _unitOfWork.AttributeRepository.FirstOrDefault((p => p.Id == publicationAttribute.AttributeId));
                                    if (attribute == null) return new { error = "El atributo no es válido" };
                                    Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == publicationAttribute.PublicationId, p => p.Subcategory);
                                    if (publication == null) return new { error = "La publicación no es válida" };

                                    if (attribute.CategoryId != publication.Subcategory.CategoryId) return new { error = "El atributo no pertenece a la categoria seleccionada" };

                                    PublicationAttribute repeated = _unitOfWork.PublicationAttributeRepository
                                    .FirstOrDefault(p => p.PublicationId == publicationAttribute.PublicationId
                                                        && p.AttributeId == publicationAttribute.AttributeId);

                                    if (repeated != null)
                                    {
                                                publicationAttribute.Id = repeated.Id;

                                                if (publicationAttribute.Value == null || publicationAttribute.Value == "")
                                                {
                                                            publicationAttribute.Deleted = true;
                                                            repeated.Deleted = true;
                                                            await this.Delete((Guid)repeated.Id);

                                                }
                                                else
                                                {
                                                            publicationAttribute.Deleted = false;
                                                            await this.Put(publicationAttribute);
                                                }
                                    }
                                    else
                                    {
                                                publicationAttribute.Id = Guid.NewGuid();
                                                if (publicationAttribute.Value == null || publicationAttribute.Value == "")
                                                {
                                                            publicationAttribute.Deleted = true;
                                                }
                                                else
                                                {
                                                            publicationAttribute.Deleted = false;

                                                }
                                                _unitOfWork.PublicationAttributeRepository.Insert(publicationAttribute);

                                    }

                                    await _unitOfWork.Save();

                                    return _unitOfWork.PublicationAttributeRepository.FirstOrDefault(v => v.Id == publicationAttribute.Id);



                        }

                        public async Task<object> Put(PublicationAttribute newPublicationAttribute)
                        {
                                    var idPublicationAttribute = newPublicationAttribute.Id;
                                    if (idPublicationAttribute == null || idPublicationAttribute.ToString().Length < 6) return new { error = "Ingrese un id de rol válido " };


                                    PublicationAttribute? PublicationAttribute = _unitOfWork.PublicationAttributeRepository.FirstOrDefault(v => v.Id == idPublicationAttribute && newPublicationAttribute.Id != null);
                                    //PublicationAttribute? PublicationAttribute = DBContext.PublicationAttributes.Where(r => r.Id == idPublicationAttribute && newPublicationAttribute.Id != null).FirstOrDefault();
                                    if (PublicationAttribute == null) return new { error = "El id no coincide con ningun rol " };

                                    //loop through each attribute entered and modify it

                                    foreach (PropertyInfo propertyInfo in newPublicationAttribute.GetType().GetProperties())
                                    {
                                                if (propertyInfo.GetValue(newPublicationAttribute) != null && propertyInfo.GetValue(newPublicationAttribute).ToString() != "")
                                                {
                                                            propertyInfo.SetValue(PublicationAttribute, propertyInfo.GetValue(newPublicationAttribute));

                                                }

                                    }

                                    _unitOfWork.PublicationAttributeRepository.Update(PublicationAttribute);
                                    await _unitOfWork.Save();
                                    //DBContext.Entry(PublicationAttribute).State = EntityState.Modified;
                                    //DBContext.SaveChanges();
                                    return PublicationAttribute;

                        }

                        public async Task<object> Delete(Guid id)
                        {

                                    PublicationAttribute publicationAttribute = _unitOfWork.PublicationAttributeRepository.FirstOrDefault(u => u.Id == id);
                                    if (publicationAttribute == null) return new { error = "El id ingresado no es válido" };
                                    publicationAttribute.Deleted = true;
                                    _unitOfWork.PublicationAttributeRepository.Update(publicationAttribute);
                                    await _unitOfWork.Save();
                                    //DBContext.Entry(PublicationAttribute).State = EntityState.Modified;
                                    //DBContext.SaveChanges();
                                    return publicationAttribute;
                        }

            }
            #endregion

}
