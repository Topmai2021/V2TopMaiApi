 
using Common.Utils.Helpers;
using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.DTOs;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Request;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Reflection;
using System.Text.RegularExpressions;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private DataContext DBContext;
        private readonly IUnitOfWork _unitOfWork;

        #region Builder
        public CategoryService(DataContext dBContext, IUnitOfWork unitOfWork)
        {
            DBContext = dBContext;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
       // public List<Category> GetAll()
           public (List<Category> categories, int totalCount) GetAll(int page = 1, int pageSize = 10)

        {

            //List<Category> categories = DBContext.Categories.OrderBy(x => x.Index).ToList();
            //foreach (Category category in categories)
            //{
            //    category.Page = 2;
            //    category.Subcategories = DBContext.Subcategories
            //                            .Where(x => x.CategoryId == category.Id && x.Deleted != true)
            //                            .OrderBy(x => x.Index).ToList();

            //    //category.Publications = DBContext.Publications.Include("Subcategory").Include("Publisher")
            //    //                        .Include("Currency").Include("Publisher.Image")
            //    //                        .Where(x => x.Subcategory.CategoryId == category.Id && x.Deleted != true && x.Status == 1)
            //    //                        .OrderByDescending(x => x.PublicationDate).Take(6).ToList();

            //    category.Publications = DBContext.Publications.Include("Subcategory")
            //                           .Include("Currency")
            //                           .Where(x => x.Subcategory.CategoryId == category.Id && x.Deleted != true && x.Status == 1)
            //                           .OrderByDescending(x => x.PublicationDate).Take(6).ToList();


            //}


           /*   List<Category> categories = _unitOfWork.CategoryRepository.FindAll(x => x.Deleted != true && x.Subcategories.Any(s => s.Deleted != true),
                   sub => sub.Subcategories).OrderBy(x => x.Index).ToList();*/


            ////Chnages

            // Calculate the number of records to skip
            int skipCount = (page - 1) * pageSize;

            // Retrieve the records using your repository with paging
            var categories = _unitOfWork.CategoryRepository
           .FindAll(x => x.Deleted != true || x.Subcategories.Any(s => s.Deleted != true), sub => sub.Subcategories)
                .OrderBy(x => x.Index)
            .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            // Get the total count of all records
            int totalCount = _unitOfWork.CategoryRepository
             .FindAll(x => x.Deleted != true || x.Subcategories.Any(s => s.Deleted != true), sub => sub.Subcategories)
                .Count();

            return (categories, totalCount);



            //foreach (Category category in categories)
            //{
            //    category.Page = 2;
            //    //category.Publications = DBContext.Publications.Include("Subcategory").Include("Publisher")
            //    //                        .Include("Currency").Include("Publisher.Image")
            //    //                        .Where(x => x.Subcategory.CategoryId == category.Id && x.Deleted != true && x.Status == 1)
            //    //                        .OrderByDescending(x => x.PublicationDate).Take(6).ToList();

            //    category.Publications = DBContext.Publications.Include("Subcategory")
            //                           .Include("Currency")
            //                           .Where(x => x.Subcategory.CategoryId == category.Id && x.Deleted != true && x.Status == 1)
            //                           .OrderByDescending(x => x.PublicationDate).Take(6).ToList();


            //}

            //Testing
          // return categories;


        }

        public Category Get(Guid id) => _unitOfWork.CategoryRepository.FirstOrDefault(u => u.Id == id);

        public async Task<bool> Post(CategoryDTO category)
        {
            var newCategory = new Category() 
            {
                Id = Guid.NewGuid(),
                Deleted = category.Deleted,
                Name= category.Name,
                IconName= category.IconName,
                UrlImage= category.UrlImage,
                Description= category.Description,
                Index= category.Index,
            };

            var categoriesLength = _unitOfWork.CategoryRepository.GetAll().Count();
            category.Index = categoriesLength + 1;

            Helper.ValidateName(category.Name);

            DBContext.Categories.Add(newCategory);

            return await _unitOfWork.Save() > 0;
        }


        public Category Delete(Guid id)
        {

          Category category = _unitOfWork.CategoryRepository.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return category;
            }

            if(category.Deleted!=false){
            _unitOfWork.CategoryRepository.Delete(category);
            
            }else{
             category.Deleted = true;
              _unitOfWork.CategoryRepository.Update(category);

            }
                _unitOfWork.SaveChanges();
           
            return category;
        }

        public List<Category> GetAllItems()
        {

            List<Category> categories = DBContext.Categories.OrderBy(x => x.Index).ToList();
            foreach (Category category in categories)
            {
                category.Subcategories = DBContext.Subcategories
                                        .Where(x => x.CategoryId == category.Id && x.Deleted != true)
                                        .OrderBy(x => x.Index).ToList();
            }
            return categories;


        }

        public object Put(UpdateRequestCategory newCategory)
        {
            var idCategory = newCategory.Id;
            if (idCategory == null || idCategory.ToString().Length < 6)
                return new { error = "Ingrese un id de categoria válido " };


            Category? category = DBContext.Categories.FirstOrDefault(c => c.Id == idCategory && newCategory.Id != null);
            if (category == null)
                return new { error = "El id no coincide con ninguna categoria " };

            //reorder index
            if (newCategory.Index != null)
            {
                var repeatedIndex = DBContext.Categories.Where(c => c.Index == newCategory.Index && c.Index != null).FirstOrDefault();
                if (repeatedIndex != null)
                {
                    if (repeatedIndex.Index < category.Index)
                    {
                        for (var i = repeatedIndex.Index; i < category.Index; i++)
                        {
                            var cat = DBContext.Categories.Where(c => c.Index == i).FirstOrDefault();
                            if (cat != null)
                                cat.Index += 1;

                        }
                    }
                    else
                    {
                        for (var i = category.Index + 1; i < repeatedIndex.Index + 1; i++)
                        {
                            var cat = DBContext.Categories.Where(c => c.Index == i).FirstOrDefault();
                            if (cat != null)
                                cat.Index -= 1;

                        }
                    }


                }
            }



            //loop through each attribute entered and modify it
            foreach (PropertyInfo propertyInfo in newCategory.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newCategory) != null && propertyInfo.GetValue(newCategory).ToString() != "")
                {

                    category.Name = newCategory.Name ?? category.Name;
                    category.Description = newCategory.Description ?? category.Description;
                    category.Index = newCategory.Index ?? category.Index;
                    category.IconName = newCategory.IconName ?? category.IconName;
                    category.UrlImage = newCategory.UrlImage ?? category.UrlImage;
                    category.Deleted = newCategory.Deleted ?? category.Deleted;

                    //propertyInfo.SetValue(category, propertyInfo.GetValue(newCategory));
                }

            }

            _unitOfWork.Save();

            return category;
        }
        #endregion
    }
}