using PhotoGalleryAPI.Models.Dtos;
using PhotoGalleryAPI.Models.Entities;

namespace PhotoGalleryAPI.Services
{
    public interface IPhotoService
    {
        Task<IEnumerable<Photo>> GetPhotosAsync();

        Task<int> CountPhotosAsync();

        Task<Photo?> FindPhotoByIdAsync(string id);

        Task DeletePhotoByIdAsync(string id);
    }
}
