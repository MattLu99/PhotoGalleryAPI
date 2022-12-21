﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhotoGalleryAPI.Models.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        [JsonIgnore]
        public virtual List<Album> Albums { get; set; } = new List<Album>();

        public DateTime LastLoginAt { get; set; }

        public DateTime RegisteredAt { get; set; }
    }
}