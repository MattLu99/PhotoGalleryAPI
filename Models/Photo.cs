﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhotoGalleryAPI.Models
{
    public class Photo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public Gallery Gallery { get; set; }

        public Guid GalleryId { get; set; }

        [StringLength(250)]
        public string Description { get; set; } = string.Empty;

        public byte[] ImageData { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}