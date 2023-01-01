using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhotoGalleryAPI.Models.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [JsonIgnore]
        public virtual List<Album> Albums { get; set; } = new List<Album>();

        public DateTime LastLoginAt { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}
