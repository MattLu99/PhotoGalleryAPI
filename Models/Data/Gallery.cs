using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhotoGalleryAPI.Models.Data
{
    public class Gallery
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public User User { get; set; }

        public Guid UserId { get; set; }

        [StringLength(100)]
        public string ParentName { get; set; } = string.Empty;

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        public List<Photo> Photos { get; set; } = new List<Photo>();

        public DateTime CreatedAt { get; set; }
    }
}
