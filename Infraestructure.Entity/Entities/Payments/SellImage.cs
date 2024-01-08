using Infraestructure.Entity.Base;
using Infraestructure.Entity.Entities.Profiles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestructure.Entity.Entities.Payments
{
    public class SellImage : IBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }

        public Sell? Sell { get; set; }

        [ForeignKey("Sell")]
        public Guid? SellId { get; set; }

        public Image? Image { get; set; }

        [ForeignKey("Image")]
        public Guid? ImageId { get; set; }

        public string? Description { get; set; }

        public Profile? UploadedBy { get; set; }
        public Guid? UploadedById { get; set; }

        public DateTime? DateTime { get; set; }

        public bool? Deleted { get; set; }

        public SellImage()
        {
        }

        ~SellImage()
        {
        }
    }//end
}