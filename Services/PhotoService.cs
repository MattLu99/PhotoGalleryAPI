using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Data;
using PhotoGalleryAPI.Models.Entities;

namespace PhotoGalleryAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly PhotoGalleryDbContext _ctx;

        public PhotoService(PhotoGalleryDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            return await _ctx.Photos.ToListAsync();
        }

        public async Task<int> CountPhotosAsync()
        {
            return await _ctx.Photos.CountAsync();
        }

        public async Task<Photo?> FindPhotoByIdAsync(string id)
        {
            return await _ctx.Photos.FindAsync(id);
        }

        public async Task DeletePhotoByIdAsync(string id)
        {
            var photo = await _ctx.Photos.FindAsync(id);
            if (photo == null)
                return;
            _ctx.Photos.Remove(photo);
            await _ctx.SaveChangesAsync();
        }
    }
}
