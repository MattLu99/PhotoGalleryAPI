using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Models;
using static System.Collections.Specialized.BitVector32;

namespace PhotoGalleryAPI.Data
{
    public class PhotoGalleryDbContext : DbContext
    {
        public PhotoGalleryDbContext(DbContextOptions<PhotoGalleryDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .HasMany<Gallery>(u => u.Galleries)
                .WithOne(g => g.User)
                .HasForeignKey(g => g.UserId)
                .HasPrincipalKey(u => u.Id);

            modelBuilder.Entity<Gallery>()
                .HasKey(g => g.Id);
            modelBuilder.Entity<Gallery>()
                .HasMany<Photo>(g => g.Photos)
                .WithOne(p => p.Gallery)
                .HasForeignKey(p => p.GalleryId)
                .HasPrincipalKey(g => g.Id);

        }
    }
}
