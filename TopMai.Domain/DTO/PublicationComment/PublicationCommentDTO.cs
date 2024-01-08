using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TopMai.Domain.DTO.PublicationComment
{
    public class PublicationCommentDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "El campo [FromId] es obligatorio.")]
        public Guid FromId { get; set; }
        [Required(ErrorMessage = "El campo [PublicationId] es obligatorio.")]
        public Guid PublicationId { get; set; }
        [JsonIgnore]
        public Guid? AnsweredPublicationCommentId { get; set; }
        [JsonIgnore]
        public DateTime? DateTime { get; set; }
        [Required(ErrorMessage = "El campo [Comment] es obligatorio.")]
        public string Comment { get; set; } = null!;
        [JsonIgnore]
        public bool Deleted { get; set; }

    }
}
