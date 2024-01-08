using Infraestructure.Entity.Entities.Posts;

namespace TopMai.Domain.Services.Post.Interfaces
{
    public interface IPostCommentService
    {
        List<PostComment> GetAll();
        PostComment Get(Guid id);
        object Post(PostComment postComment);
        object Put(PostComment newPostComment);
        object Delete(Guid id);
    }
}
