using Infraestructure.Entity.DTOs;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Request;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface ICategoryService
    {
         //List<Category> GetAll();
        (List<Category> categories, int totalCount) GetAll(int page = 1, int pageSize = 10);
        Category Get(Guid id);
        Task<bool> Post(CategoryDTO category);
        Category Delete(Guid id);
        object Put(UpdateRequestCategory newCategory);
    }
}
