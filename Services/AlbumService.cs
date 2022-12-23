using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Data;
using PhotoGalleryAPI.Models.Dtos;
using PhotoGalleryAPI.Models.Entities;

namespace PhotoGalleryAPI.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly PhotoGalleryDbContext _ctx;

        public AlbumService(PhotoGalleryDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<Album>> GetAlbumsAsync()
        {
            return await _ctx.Albums.ToListAsync();
        }

        public async Task<int> CountAlbumsAsync()
        {
            return await _ctx.Albums.CountAsync();
        }

        public async Task<Album?> FindAlbumByIdAsync(string id)
        {
            return await _ctx.Albums.FindAsync(id);
        }

        public async Task<Photo> CreateNewPhotoInAlbum(Album album, PhotoDto photoDto)
        {
            var newPhoto = new Photo()
            {
                Id = Guid.NewGuid().ToString(),
                Name = photoDto.Name,
                Album = album,
                AlbumId = album.Id,
                Description = photoDto.Description,
                TakenAtTime = photoDto.TakenAtTime,
                TakenAtLocation = photoDto.TakenAtLocation,
                ImageData = photoDto.ImageData,
                UploadedAt = DateTime.Now
            };
            album.Photos.Add(newPhoto);
            _ctx.Photos.Add(newPhoto);

            await _ctx.SaveChangesAsync();

            return newPhoto;
        }

        public async Task<Photo?> ChangeCoverImagebyIdAsync(Album album, string coverImageId)
        {
            var photo = album.Photos.FindLast(p => p.Id.Equals(coverImageId));
            if (photo == null)
                return null;

            album.CoverImageId = photo.Id;
            await _ctx.SaveChangesAsync();

            return photo;
        }

        public async Task DeleteAlbumByIdAsync(string id)
        {
            var album = await _ctx.Albums.FindAsync(id);
            if (album == null)
                return;
            _ctx.Albums.Remove(album);
            await _ctx.SaveChangesAsync();
        }
    }
}
