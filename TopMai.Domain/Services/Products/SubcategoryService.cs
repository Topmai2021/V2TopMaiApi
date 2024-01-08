using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Products;
using System.Reflection;
using System.Text.RegularExpressions;
using TopMai.Domain.DTO.SubCategory;
using TopMai.Domain.Services.Products.Interfaces;


namespace TopMai.Domain.Services.Products
{
    public class SubcategoryService : ISubcategoryService
    {
        private IUnitOfWork _unitOfWork;
        private DataContext _dataContext;

        #region Builder
        public SubcategoryService(IUnitOfWork unitOfWork, DataContext dataContext)
        {
            _unitOfWork = unitOfWork;
            _dataContext = dataContext;
        }
        #endregion

        #region Methods
        // public List<Subcategory> GetAll() => _unitOfWork.SubcategoryRepository.GetAll().OrderBy(x => x.Index).ToList();
        // public List<Subcategory> GetAll(int page = 1, int pageSize = 10)


        public object GetAll(int page = 1, int pageSize = 10)
        {
            
            int skipCount = (page - 1) * pageSize;

            var subcategories = _unitOfWork.SubcategoryRepository
                .GetAll()
                .OrderBy(x => x.Index)
                .Skip(skipCount)
                .Take(pageSize)
                 .Select(subcategory => new
        {
            subcategory.Id,
            subcategory.Name,
            subcategory.Deleted,
            subcategory.CategoryId,
            subcategory.SubcategoryId,
            subcategory.UrlImage,
            subcategory.UrlSecondaryImage,
            subcategory.Index,
            subcategory.IndexMostUsed,
            subcategory.IconName,
            subcategory.UrlInternalImage,
            Category = _unitOfWork.CategoryRepository
        .FindAll(x => x.Id == subcategory.CategoryId)
        .FirstOrDefault()
        })
                .ToList();


            int totalCount = _dataContext.Subcategories.Count(); // Counting non-deleted records

            var response = new
            {
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                Data = subcategories
            };

            return response;

        }

        public Subcategory Get(Guid id) => _unitOfWork.SubcategoryRepository.FirstOrDefault(u => u.Id == id);

        public object Post(SubCategoryDTO subcategory)
        {
            var newSubCategoty = new Subcategory()
            {
                Id = Guid.NewGuid(),
                Name = subcategory.Name,
                CategoryId = subcategory.CategoryId,
                SubcategoryId = subcategory.SubcategoryId,
                UrlImage = subcategory.UrlImage,
                Deleted = subcategory.Deleted,
                IconName = subcategory.IconName,
                Index = subcategory.Index,
                UrlInternalImage = subcategory.UrlInternalImage,
                IndexMostUsed = subcategory.IndexMostUsed,
                UrlSecondaryImage = subcategory.UrlSecondaryImage,
            };

            var subcategoriesLength = _unitOfWork.SubcategoryRepository.GetAll().Count();
            newSubCategoty.Index = subcategoriesLength + 1;

            if (newSubCategoty.Name == null || newSubCategoty.Name.Length < 4)
                return new { error = "El nombre de la subcategoria debe ser al menos 3 caracteres" };

            if (!Regex.Match(newSubCategoty.Name, "^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$").Success)
                return new { error = "El nombre no puede tener caracteres especiales" };



            if (newSubCategoty.CategoryId != null && newSubCategoty.SubcategoryId != null)
                return new { error = "Una subcategoria no puede estar asociada a una categoria y subcategoria al mismo tiempo" };

            Category? category = _unitOfWork.CategoryRepository.FirstOrDefault(c => c.Id == newSubCategoty.CategoryId);
            if (subcategory.CategoryId != null && category == null)
                return new { error = "Ninguna categoria coincide con la id categoria ingresada" };

            Subcategory asociatedSubcategory = _unitOfWork.SubcategoryRepository.FirstOrDefault(s => s.Id == newSubCategoty.SubcategoryId);
            if (subcategory.SubcategoryId != null && asociatedSubcategory == null)
                return new { error = "Ninguna subcategoria coincide con la id subcategoria ingresada" };


            _unitOfWork.SubcategoryRepository.Insert(newSubCategoty);
            _unitOfWork.Save();

            return (newSubCategoty);
        }


        public object Delete(Guid id)
        {

          Subcategory subcategory = _unitOfWork.SubcategoryRepository.FirstOrDefault(c => c.Id == id);

            if (subcategory == null)
            {
                return subcategory;
            }

            if(subcategory.Deleted==true){
                          _unitOfWork.SubcategoryRepository.Delete(subcategory);
            }else{
             subcategory.Deleted = true;
              _unitOfWork.SubcategoryRepository.Update(subcategory);
            }
  
              _unitOfWork.SaveChanges();
           
            return subcategory;
        }


        public object GetSubcategoriesBySubcategoryId(Guid id)
        {
            if (id == null || id.ToString().Length < 6)
                return new { error = "Ingrese un id válida" };

            List<Subcategory> subcategories = _unitOfWork.SubcategoryRepository.GetAll().Where(s => s.SubcategoryId == id && s.Deleted != true).OrderBy(sb => sb.Index).ToList();
            return subcategories;
        }


        internal class Counter
        {
            public Guid SubCategoryId { get; set; }
            public int Count { get; set; }
        }

        public object getSubcategoriesMostUsed()
        {
            Category category = new Category();
            category.Name = "Inicio";
            category.Subcategories = new List<Subcategory>();
            List<Subcategory> subcategories = _unitOfWork.SubcategoryRepository.GetAll().Where(s => s.Deleted != true && s.IndexMostUsed >= 0 && s.IndexMostUsed < 20).OrderBy(s => s.IndexMostUsed).Take(20).ToList();
            category.Subcategories = subcategories;

            return category;
        }

        public object Put(SubCategoryDTO subcategory)
        {
            //var idSubcategory = updatedSubcategory.Id;
            //if (idSubcategory == null || idSubcategory.ToString().Length < 6)
            //    return new { error = "Ingrese un id de subcategoria válido" };

            //Subcategory subcategory = _unitOfWork.SubcategoryRepository.FirstOrDefault(s => s.Id == idSubcategory && updatedSubcategory.Id != null);
            //if (subcategory == null)
            //    return new { error = "El id no coincide con ninguna subcategoria" };

            //foreach (var propertyInfo in updatedSubcategory.GetType().GetProperties())
            //{
            //    if (propertyInfo.GetValue(updatedSubcategory) != null && propertyInfo.GetValue(updatedSubcategory).ToString() != "")
            //    {
            //        propertyInfo.SetValue(subcategory, propertyInfo.GetValue(updatedSubcategory));
            //    }
            //}
            /* var newSubCategoty = new Subcategory()
             {
                 Id = Guid.NewGuid(),
                 Name = subcategory.Name,
                 CategoryId = subcategory.CategoryId,
                 SubcategoryId = subcategory.SubcategoryId,
                 UrlImage = subcategory.UrlImage,
                 Deleted = subcategory.Deleted,
                 IconName = subcategory.IconName,
                 Index = subcategory.Index,
                 UrlInternalImage = subcategory.UrlInternalImage,
                 IndexMostUsed = subcategory.IndexMostUsed,
                 UrlSecondaryImage = subcategory.UrlSecondaryImage,
             };

             _unitOfWork.Update();

             return subcategory;*/
            var existingSubcategory = _unitOfWork.SubcategoryRepository.FirstOrDefault(s => s.Id == subcategory.Id);

            if (existingSubcategory == null)
            {
                // Handle if the entity doesn't exist
                return new { error = "Subcategory not found" };
            }

            // Update properties of the existing entity with the new values
            existingSubcategory.Name = subcategory.Name;
            existingSubcategory.Deleted = subcategory.Deleted;
            existingSubcategory.IconName = subcategory.IconName;
            existingSubcategory.UrlImage = subcategory.UrlImage;
            existingSubcategory.CategoryId = subcategory.CategoryId;
            // ... update other properties

            // Save changes to persist the updates
            _unitOfWork.SubcategoryRepository.Update(existingSubcategory);
            _unitOfWork.Save(); // Or a similar method for saving changes

            return existingSubcategory;
        }

        public object GetSubcategoriesByCategory(Guid id)
        {
            if (id == null || id.ToString().Length < 6)
                return new { error = "Ingrese un id válida" };
            List<Subcategory> subcategories = _unitOfWork.SubcategoryRepository.GetAll().Where(s => s.CategoryId == id && s.Deleted != true).OrderBy(sb => sb.Index).ToList(); ;

            return subcategories;
        }
        #endregion
    }
}
