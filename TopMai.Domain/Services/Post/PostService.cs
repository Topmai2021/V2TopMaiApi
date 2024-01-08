using Infraestructure.Core.Data;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Posts;
using Infraestructure.Entity.Entities.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TopMai.Domain.Services.Post.Interfaces;
using TopMai.Domain.Services.Profiles;

namespace TopMai.Domain.Services.Post
{
    public class PostService : IPostService
    {
        private readonly DataContext DBContext;
        private readonly IUnitOfWork _unitOfWork;

        #region Builder
        public PostService(DataContext dBContext, IUnitOfWork unitOfWork)
        {
            this.DBContext = dBContext;
            this._unitOfWork = unitOfWork;
        }
        #endregion


        #region Methods
        public List<Infraestructure.Entity.Entities.Posts.Post> GetAll()
        {
            List<Infraestructure.Entity.Entities.Posts.Post> posts = DBContext.Posts.OrderByDescending(x => x.Id).ToList();

            return posts;
        }

        public Infraestructure.Entity.Entities.Posts.Post Get(Guid id)
        {
            var post = DBContext.Posts.FirstOrDefault(u => u.Id == id);
            post.Images = (List<Image>?)this.GetImagesByPost(post);
            post.Likes = (List<PostLike>?)this.GetLikesByPost(post);
            post.LikesAmount = post.Likes.Count();
            post.Comments = (List<PostComment>?)this.GetCommentsByPost(post);

            return post;

        }

        public object Post(Infraestructure.Entity.Entities.Posts.Post post)
        {
            if (post.Content == null || post.Content.Length < 1) return new { error = "El contenido no puede estar vacio" };
            if (post.PublisherId == null) return new { error = "El id de publicante no puede ser nulo" };
            Profile profile = DBContext.Profiles.Where(p => p.Id == post.PublisherId).FirstOrDefault();
            if (profile == null) return new { error = "No se encuentra ningun perfil con la id ingresada" };

            post.Id = Guid.NewGuid();
            post.Deleted = false;
            post.PublicationDate = DateTime.Now;

            DBContext.Posts.Add(post);
            DBContext.SaveChanges();

            return DBContext.Posts.Where(p => p.Id == post.Id).First();
        }

        public object Put(Infraestructure.Entity.Entities.Posts.Post newPost)
        {
            var idPost = newPost.Id;
            if (idPost == null || idPost.ToString().Length < 6)
                return new { error = "Ingrese un id de post válido " };

            Infraestructure.Entity.Entities.Posts.Post post = DBContext.Posts.Where(r => r.Id == idPost && newPost.Id != null).FirstOrDefault();
            if (post == null)
                return new { error = "El id no coincide con ningun post " };

            //loop through each attribute entered and modify it

            foreach (PropertyInfo propertyInfo in newPost.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(newPost) != null && propertyInfo.GetValue(newPost).ToString() != "")
                    propertyInfo.SetValue(post, propertyInfo.GetValue(newPost));

            }

            DBContext.Entry(post).State = EntityState.Modified;
            DBContext.SaveChanges();

            return post;
        }

        public List<PostComment>? GetCommentsByPost(Infraestructure.Entity.Entities.Posts.Post post)
        {
            List<PostComment>? postComments = DBContext.PostComments.Where(p => p.PostId == post.Id && p.Deleted != true).OrderByDescending(p => p.DateTime).ToList();
            if (postComments == null) return null;
            foreach (var comment in postComments)
            {
                comment.From = DBContext.Profiles.Where(p => p.Id == comment.FromId).FirstOrDefault();
                if (comment.From != null)
                {
                    Image image = DBContext.Images.Where(i => i.Id == comment.From.ImageId).FirstOrDefault();
                    if (image != null)
                        comment.From.Image = image;
                }

            }

            return postComments;
        }

        public object GetLikesByPost(Infraestructure.Entity.Entities.Posts.Post post)
        {
            List<PostLike> postLikes = DBContext.PostLikes.Where(p => p.PostId == post.Id && p.Deleted != true).ToList();
            if (postLikes == null)
                return null;
            foreach (var like in postLikes)
            {
                like.From = DBContext.Profiles.FirstOrDefault(p => p.Id == like.FromId);
                if (like.From != null)
                {
                    Image image = DBContext.Images.FirstOrDefault(i => i.Id == like.From.ImageId);
                    if (image != null)
                        like.From.Image = image;
                }
            }
            return postLikes;
        }
        public object GetPostByProfileId(Guid id, Guid? userId)
        {
            if (id == null)
                return new { error = "El id no puede ser nulo" };

            Profile prof = DBContext.Profiles.Where(p => p.Id == id).FirstOrDefault();
            if (prof == null)
                return new { error = "El id ingresado no pertenece a ningun perfil" };

            List<Infraestructure.Entity.Entities.Posts.Post> posts = DBContext.Posts.Where(p => p.PublisherId == id
                                                                                             && p.Deleted != true)
                                                                                     .OrderByDescending(p => p.PublicationDate).ToList();

            List<Infraestructure.Entity.Entities.Posts.Post> postsToView = new List<Infraestructure.Entity.Entities.Posts.Post>();
            bool isMyUser = false;
            if (id == userId) isMyUser = true;
            foreach (Infraestructure.Entity.Entities.Posts.Post post in posts)
            {

                bool isMutual = false;
                if (userId != null)
                {
                    Contact me = DBContext.Contacts.Where(c => c.ProfileId == id
                                && c.ContactProfileId == userId).FirstOrDefault();
                    if (me != null)
                    {
                        isMutual = true;
                    }

                }

                if ((isMyUser) || (post.Private == 0) || (post.Private == 2 && isMutual))
                {
                    post.Images = (List<Image>)GetImagesByPost(post);
                    post.Comments = (List<PostComment>)GetCommentsByPost(post);
                    post.Likes = (List<PostLike>)GetLikesByPost(post);
                    post.LikesAmount = post.Likes.Count;

                    postsToView.Add(post);
                }
            }

            return postsToView;
        }


        public object GetImagesByPost(Infraestructure.Entity.Entities.Posts.Post post)
        {
            List<PostImage> postImages = DBContext.PostImages.Where(p => p.PostId == post.Id && p.Deleted != true).ToList();
            List<Image> images = new List<Image>();
            foreach (PostImage postImage in postImages)
            {
                if (postImage.ImageId != null)
                {
                    Image image = DBContext.Images.Where(i => i.Id == postImage.ImageId).FirstOrDefault();
                    if (image != null)
                    {
                        images.Add(image);
                    }
                }
            }

            return images;
        }
        public object AddImageToPost(Guid idImage, Guid idPost)
        {
            if (idImage == null)
                return new { error = "El id de imagen no puede ser nulo" };
            if (idPost == null)
                return new { error = "El id de post no puede ser nulo" };

            Image image = DBContext.Images.Where(i => i.Id == idImage).FirstOrDefault();
            if (image == null)
                return new { error = "El id de imagen no pertenece a ninguna imagen" };
            Infraestructure.Entity.Entities.Posts.Post post = DBContext.Posts.Where(p => p.Id == idPost).FirstOrDefault();

            if (post == null)
                return new { error = "El id de post no pertenece a ningun post" };

            PostImage postImage = new PostImage();
            postImage.Id = Guid.NewGuid();
            postImage.ImageId = idImage;
            postImage.PostId = idPost;
            postImage.Deleted = false;

            DBContext.PostImages.Add(postImage);


            DBContext.Entry(post).State = EntityState.Modified;
            DBContext.SaveChanges();

            return post;
        }

        public object GetContactsPostByProfileId(Guid idProfile)
        {
            var prof = _unitOfWork.ProfileRepository.FindAll(x => x.Id == idProfile,
                                                                       c => c.Contacts);

            if (prof == null)
                return new { error = "Debe completar su perfil" };

            List<Contact> contacts = _unitOfWork.ContactRepository.FindAll(c => c.ProfileId == idProfile).ToList();


            List<Infraestructure.Entity.Entities.Posts.Post> posts = new List<Infraestructure.Entity.Entities.Posts.Post>();

            foreach (Contact contact in contacts)
            {
                bool isMutual = false;
                Contact me = _unitOfWork.ContactRepository.FirstOrDefault(c => c.ProfileId == contact.Id
                                                    && c.ContactProfileId == idProfile);
                if (me != null)
                {
                    isMutual = true;
                }

                Guid friendId = (Guid)contact.ContactProfileId;
                var contactPosts = (List<Infraestructure.Entity.Entities.Posts.Post>)GetPostByProfileId(friendId, idProfile);

                foreach (Infraestructure.Entity.Entities.Posts.Post post in contactPosts)
                {
                    // 1 - public - 2 only contacts
                    if (post.Private == 0 || (post.Private == 2 && isMutual))
                    {
                        posts.Add(post);
                    }
                }
            }

            // add my post
            posts.AddRange((List<Infraestructure.Entity.Entities.Posts.Post>)GetPostByProfileId(idProfile, idProfile));

            return posts.OrderByDescending(p => p.PublicationDate).ToList();

        }


        public object Delete(Guid id)
        {
            Infraestructure.Entity.Entities.Posts.Post post = DBContext.Posts.FirstOrDefault(u => u.Id == id);
            if (post == null) return new { error = "El id ingresado no es válido" };
            post.Deleted = true;
            DBContext.Entry(post).State = EntityState.Modified;
            DBContext.SaveChanges();
            return post;
        }
        #endregion
    }
}
