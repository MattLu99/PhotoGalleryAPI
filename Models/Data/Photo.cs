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
        public virtual Album Album { get; set; }

        public Guid AlbumId { get; set; }

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        public DateTime TakenAtTime { get; set; }

        public string TakenAtLocation { get; set; } = string.Empty;

        //Json format issue with .net support: maybe this should be array of bytes
        public string ImageData { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; }
    }
}
