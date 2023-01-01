using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhotoGalleryAPI.Models.Entities
{
    public class Photo
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual Album Album { get; set; } = new Album();

        public string AlbumId { get; set; } = string.Empty;

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        public DateTime TakenAtTime { get; set; }

        public string TakenAtLocation { get; set; } = string.Empty;

        [MaxLength]
        public string ImageData { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; }
    }
}
