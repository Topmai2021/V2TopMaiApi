using Infraestructure.Entity.Entities.Posts;

namespace TopMai.Domain.Services.Post.Interfaces
{
    public interface IPostService
    {
        List<Infraestructure.Entity.Entities.Posts.Post> GetAll();
        Infraestructure.Entity.Entities.Posts.Post Get(Guid id);
        object Post(Infraestructure.Entity.Entities.Posts.Post post);
        object Put(Infraestructure.Entity.Entities.Posts.Post newPost);
        List<PostComment>? GetCommentsByPost(Infraestructure.Entity.Entities.Posts.Post post);
        object GetLikesByPost(Infraestructure.Entity.Entities.Posts.Post post);
        object GetPostByProfileId(Guid id, Guid? userId);
        object GetImagesByPost(Infraestructure.Entity.Entities.Posts.Post post);
        object AddImageToPost(Guid idImage, Guid idPost);
        object GetContactsPostByProfileId(Guid idProfile);
        object Delete(Guid id);
    }
}
