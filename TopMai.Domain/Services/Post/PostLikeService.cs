using Infraestructure.Core.Data;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopMai.Domain.Services.Post.Interfaces;

namespace TopMai.Domain.Services.Post
{
    public class PostLikeService : IPostLikeService
    {
        private DataContext DBContext;
        #region Builder
        public PostLikeService(DataContext dBContext)
        {
            this.DBContext = dBContext;

        }
        #endregion

        #region Methods
        public List<PostLike> GetAll()
        {
            List<PostLike> postLikes = DBContext.PostLikes.OrderByDescending(x => x.Id).ToList();

            return postLikes;
        }

        public PostLike Get(Guid id)
        {
            return DBContext.PostLikes.FirstOrDefault(u => u.Id == id);
        }

        public object Post(PostLike postLike)
        {
            Profile profile = DBContext.Profiles.Where(p => p.Id == postLike.FromId).FirstOrDefault();
            if (profile == null) return new { error = "El usuario no es válido" };
            Infraestructure.Entity.Entities.Posts.Post post = DBContext.Posts.Where(p => p.Id == postLike.PostId).FirstOrDefault();
            if (post == null) return new { error = "El post no es válido" };

            PostLike repeated = DBContext.PostLikes.Where(p => p.PostId == postLike.PostId && p.FromId == postLike.FromId).FirstOrDefault();
            if (repeated != null)
            {
                repeated.Deleted = !repeated.Deleted;
                DBContext.Entry(repeated).State = EntityState.Modified;
                DBContext.SaveChanges();
                return repeated;


            }  
            
            else
            {
                postLike.Id = Guid.NewGuid();
                postLike.Deleted = false;

                DBContext.PostLikes.Add(postLike);
                DBContext.SaveChanges();
            }

            return DBContext.PostLikes.Where(p => p.Id == postLike.Id).FirstOrDefault();


        }

        public object Put(PostLike newPostLike)
        {
            var idPostLike = newPostLike.Id;
            if (idPostLike == null || idPostLike.ToString().Length < 6) return new { error = "Ingrese un id de like válido " };

            PostLike? postLike = DBContext.PostLikes.Where(r => r.Id == idPostLike && newPostLike.Id != null).FirstOrDefault();
            if (postLike == null) return new { error = "El id no coincide con ningun like " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newPostLike.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newPostLike) != null && propertyInfo.GetValue(newPostLike).ToString() != "")
                    propertyInfo.SetValue(postLike, propertyInfo.GetValue(newPostLike));

            }

            DBContext.Entry(postLike).State = EntityState.Modified;
            DBContext.SaveChanges();

            return postLike;
        }

        public object Delete(Guid id)
        {
            PostLike postLike = DBContext.PostLikes.FirstOrDefault(u => u.Id == id);
            if (postLike == null) return new { error = "El id ingresado no es válido" };
            postLike.Deleted = true;
            DBContext.Entry(postLike).State = EntityState.Modified;
            DBContext.SaveChanges();

            return postLike;
        }
        #endregion
    }
}
