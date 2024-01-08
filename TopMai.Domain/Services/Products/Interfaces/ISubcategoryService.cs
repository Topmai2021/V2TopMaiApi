using Infraestructure.Entity.Entities.Products;
using TopMai.Domain.DTO.SubCategory;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface ISubcategoryService
    {
        //List<Subcategory> GetAll();
        object GetAll(int page = 1, int pageSize = 10);
        //List<Subcategory> GetAll(int page = 1, int pageSize = 10);
        Subcategory Get(Guid id);
        object Post(SubCategoryDTO subcategory);
        object Delete(Guid id);
        object GetSubcategoriesBySubcategoryId(Guid id);
        object getSubcategoriesMostUsed();
        // object Put(SubCategoryDTO newSubcategory);
        object Put(SubCategoryDTO updatedSubcategory);
        object GetSubcategoriesByCategory(Guid id);
    }
}
