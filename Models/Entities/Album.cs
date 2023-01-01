using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhotoGalleryAPI.Models.Entities
{
    public class Album
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual User User { get; set; } = new User();

        public string UserId { get; set; } = string.Empty;

        [StringLength(100)]
        public string ParentName { get; set; } = string.Empty;

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual List<Photo> Photos { get; set; } = new List<Photo>();

        public string? CoverImageId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
