using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Chats
{
    public partial class RoomOfConversation
    {
        public Guid Id { get; set; }
        public bool? Deleted { get; set; }
        public bool? BrokenConversation { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UserOneId { get; set; }
        public Guid? UserTwoId { get; set; }
        public Guid? PublicationId { get; set; }

        [NotMapped]
        public string? PublicationName { get; set; }

        [NotMapped]
        public string? PublicationImgUrl { get; set; }

        [NotMapped]
        public Profile? ProfileOne { get; set; }

        [NotMapped]
        public Profile? ProfileTwo { get; set; }
    }
}