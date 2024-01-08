namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IAttributeService
    {
        List<Infraestructure.Entity.Entities.Products.Attribute> GetAll();
        Infraestructure.Entity.Entities.Products.Attribute Get(Guid id);
        object Post(Infraestructure.Entity.Entities.Products.Attribute attribute);
        object Put(Infraestructure.Entity.Entities.Products.Attribute newAttribute);
        object Delete(Guid id);

        Task<object> GetAttributesByCategoryId(Guid id);
    }
}
