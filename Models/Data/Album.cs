using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhotoGalleryAPI.Models.Data
{
    public class Album
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual User User { get; set; }

        public string UserId { get; set; }

        [StringLength(100)]
        public string ParentName { get; set; } = string.Empty;

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual List<Photo> Photos { get; set; } = new List<Photo>();

        public DateTime CreatedAt { get; set; }
    }
}
