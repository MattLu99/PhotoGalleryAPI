using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Models;

namespace PhotoGalleryAPI.Data
{
    public class PhotoGalleryDbContext : DbContext
    {
        public PhotoGalleryDbContext(DbContextOptions<PhotoGalleryDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
