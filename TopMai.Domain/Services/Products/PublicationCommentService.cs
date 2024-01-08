using Common.Utils.Exceptions;
using Infraestructure.Core.UnitOfWork.Interface;
using Infraestructure.Entity.Entities.Products;
using Infraestructure.Entity.Entities.Profiles;
using TopMai.Domain.DTO.PublicationComment;
using TopMai.Domain.Services.Products.Interfaces;

namespace TopMai.Domain.Services.Products
{
    public class PublicationCommentService : IPublicationCommentService
    {
        #region attributes
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Builder
        public PublicationCommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public PublicationComment GetPublicationComment(Guid idPublicationComment) => _unitOfWork.PublicationCommentRepository.FirstOrDefault(u => u.Id == idPublicationComment);

        public async Task<bool> Post(PublicationCommentDTO publicationComment)
        {
            Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == publicationComment.FromId);
            if (profile == null)
                throw new BusinessException("No se encuentra ningun perfil con la id ingresada");

            Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == publicationComment.PublicationId);
            if (publication == null)
                throw new BusinessException("No se encuentra ninguna publicación con la id ingresada");

            if (string.IsNullOrEmpty(publicationComment.Comment))
                throw new BusinessException("Ingrese un comentario válido");

            if (publicationComment.AnsweredPublicationCommentId != null)
            {
                PublicationComment answeredPublicationComment = _unitOfWork.PublicationCommentRepository.FirstOrDefault(p => p.Id == publicationComment.AnsweredPublicationCommentId);
                if (answeredPublicationComment == null)
                    throw new BusinessException("No se encuentra ningun comentario con la id ingresada");
            }
            var newComment = new PublicationComment
            {
                Id = Guid.NewGuid(),
                FromId = publicationComment.FromId,
                PublicationId = publicationComment.PublicationId,
                DateTime = DateTime.Now,
                Comment = publicationComment.Comment,
            };

            _unitOfWork.PublicationCommentRepository.Insert(newComment);

            return await _unitOfWork.Save() > 0;
        }

        public async Task<bool> Put(PublicationCommentDTO newPublicationComment)
        {
            PublicationComment? publicationComment = GetPublicationComment(newPublicationComment.Id);
            if (publicationComment == null)
                throw new BusinessException("El id no coincide con ningun carrito");

            publicationComment.Comment = newPublicationComment.Comment;
            publicationComment.DateTime = DateTime.Now;

            return await _unitOfWork.Save() > 0;
        }

        public object GetPublicationCommentsByPublication(Guid idPublication)
        {
            Publication publication = _unitOfWork.PublicationRepository.FirstOrDefault(p => p.Id == idPublication);
            if (publication == null)
                return new { error = "No se encuentra ninguna publicación con la id ingresada" };

            List<PublicationComment> publicationComments = _unitOfWork.PublicationCommentRepository.FindAll(x => x.PublicationId == idPublication).OrderByDescending(o => o.DateTime).ToList();
            foreach (PublicationComment publicationComment in publicationComments)
            {
                Profile profile = _unitOfWork.ProfileRepository.FirstOrDefault(p => p.Id == publicationComment.FromId);
                if (profile != null)
                {
                    publicationComment.From = profile;
                    publicationComment.ProfileReview = _unitOfWork.ProfileReviewRepository.FirstOrDefault(p => p.FromId == profile.Id
                                                                                                            && p.ToId == publication.PublisherId);
                    publicationComment.From.Image = _unitOfWork.ImageRepository.FirstOrDefault(i => i.Id == profile.ImageId);
                }
            }

            return publicationComments;
        }

        public async Task<bool> Delete(Guid idPublicationComment)
        {
            PublicationComment publicationComment = GetPublicationComment(idPublicationComment);
            if (publicationComment == null)
                throw new BusinessException("No se encontró pubicación a eliminar.");

            _unitOfWork.PublicationCommentRepository.Delete(publicationComment);
            return await _unitOfWork.Save() > 0;
        }


        #endregion

    }
}
