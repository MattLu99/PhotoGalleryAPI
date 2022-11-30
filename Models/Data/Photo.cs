using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhotoGalleryAPI.Models.Data
{
    public class Photo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual Gallery Gallery { get; set; }

        public Guid GalleryId { get; set; }

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        public DateTime TakenAtTime { get; set; }

        public string TakenAtLocation { get; set; } = string.Empty;

        public byte[]? ImageData { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
