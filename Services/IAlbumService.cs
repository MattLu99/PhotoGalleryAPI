using PhotoGalleryAPI.Models.Dtos;
using PhotoGalleryAPI.Models.Entities;

namespace PhotoGalleryAPI.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAlbumsAsync();

        Task<int> CountAlbumsAsync();

        Task<Album?> FindAlbumByIdAsync(string id);

        Task<Photo> CreateNewPhotoInAlbum(Album album, PhotoDto photoDto);

        Task<Photo?> ChangeCoverImagebyIdAsync(Album album, string coverImageId);

        Task DeleteAlbumByIdAsync(string id);
    }
}
