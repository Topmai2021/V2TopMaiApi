using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Post.Interfaces;

namespace TopMai.Domain.Services.Post
{
    public class PostCommentService : IPostCommentService
    {
        private DataContext DBContext;
        #region Builder
        public PostCommentService(DataContext dBContext)
        {
            this.DBContext = dBContext;

        }
        #endregion

        #region Methods
        public List<PostComment> GetAll()
        {
            List<PostComment> postComments = DBContext.PostComments.OrderByDescending(x => x.Id).ToList();

            return postComments;
        }

        public PostComment Get(Guid id)
        {
            return DBContext.PostComments.FirstOrDefault(u => u.Id == id);
        }

        public object Post(PostComment postComment)
        {
            Profile profile = DBContext.Profiles.Where(p => p.Id == postComment.FromId).FirstOrDefault();
            if (profile == null) return new { error = "El usuario no es válido" };
            Infraestructure.Entity.Entities.Posts.Post post = DBContext.Posts.Where(p => p.Id == postComment.PostId).FirstOrDefault();
            if (post == null) return new { error = "El post no es válido" };
            if (postComment.Comment == null || postComment.Comment.Length < 1) return new { error = "El comentario no puede estar vacio" };

            postComment.Id = Guid.NewGuid();
            postComment.Deleted = false;

            DBContext.PostComments.Add(postComment);
            DBContext.SaveChanges();

            return DBContext.PostComments.Where(r => r.Id == postComment.Id).First();
        }

        public object Put(PostComment newPostComment)
        {
            var idPostComment = newPostComment.Id;
            if (idPostComment == null || idPostComment.ToString().Length < 6) return new { error = "Ingrese un id de comentario válido " };

            PostComment? postComment = DBContext.PostComments.Where(r => r.Id == idPostComment && newPostComment.Id != null).FirstOrDefault();
            if (postComment == null) return new { error = "El id no coincide con ningun comentario " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newPostComment.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newPostComment) != null && propertyInfo.GetValue(newPostComment).ToString() != "")
                    propertyInfo.SetValue(postComment, propertyInfo.GetValue(newPostComment));
            }

            DBContext.Entry(postComment).State = EntityState.Modified;
            DBContext.SaveChanges();

            return postComment;
        }

        public object Delete(Guid id)
        {
            PostComment postComment = DBContext.PostComments.FirstOrDefault(u => u.Id == id);
            if (postComment == null) return new { error = "El id ingresado no es válido" };
            postComment.Deleted = true;
            DBContext.Entry(postComment).State = EntityState.Modified;
            DBContext.SaveChanges();

            return postComment;
        }
        #endregion
    }
}
