using Infraestructure.Entity.Entities.Products;
using TopMai.Domain.DTO.PublicationComment;

namespace TopMai.Domain.Services.Products.Interfaces
{
    public interface IPublicationCommentService
    {
        PublicationComment GetPublicationComment(Guid idPublicationComment);
        Task<bool> Post(PublicationCommentDTO publicationComment);
        Task<bool> Put(PublicationCommentDTO newPublicationComment);
        object GetPublicationCommentsByPublication(Guid idPublication);
        Task<bool> Delete(Guid idPublicationComment);

    }
}
