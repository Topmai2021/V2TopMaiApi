using System.ComponentModel.DataAnnotations;

namespace TopMai.Domain.DTO.Chats
{
    public class AddMessageDto
    {
        [Required]
        public Guid FromId { get; set; }
        [Required]
        public Guid ToId { get; set; }
        [Required(ErrorMessage = "El contenido del mensaje es requeido.")]
        public string Content { get; set; } = null!;
        [Required]
        public Guid ChatId { get; set; }
        [Required]
        public int MessageTypeId { get; set; }
    }
}
