using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Models.Data;

namespace PhotoGalleryAPI.Data
{
    public class PhotoGalleryDbContext : DbContext
    {
        public PhotoGalleryDbContext(DbContextOptions<PhotoGalleryDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
