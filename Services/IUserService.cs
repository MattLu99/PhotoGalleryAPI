using PhotoGalleryAPI.Models.Dtos;
using PhotoGalleryAPI.Models.Entities;

namespace PhotoGalleryAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<int> CountUsersAsync();

        Task<User?> FindUserByIdAsync(string id);

        Task<User?> FindUserByNameAsync(string name);

        bool VerifyUserPassword(string password, User user);

        Task<User> CreateNewUserAsync(UserDto userDto);

        Task<Album> CreateNewAlbumForUserAsync(User user, AlbumDto albumDto);

        Task DeleteUserByIdAsync(string id);
    }
}
