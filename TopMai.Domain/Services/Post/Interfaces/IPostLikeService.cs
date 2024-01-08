using Infraestructure.Entity.Entities.Posts;

namespace TopMai.Domain.Services.Post.Interfaces
{
    public interface IPostLikeService
    {
        List<PostLike> GetAll();
        PostLike Get(Guid id);
        object Post(PostLike postLike);
        object Put(PostLike newPostLike);
        object Delete(Guid id);
    }
}
