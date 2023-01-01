using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Data;
using PhotoGalleryAPI.Models.Dtos;
using PhotoGalleryAPI.Models.Entities;
using System.Security.Cryptography;

namespace PhotoGalleryAPI.Services
{
    public class UserService : IUserService
    {
        private readonly PhotoGalleryDbContext _ctx;

        public UserService(PhotoGalleryDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _ctx.Users.ToListAsync();
        }

        public async Task<int> CountUsersAsync()
        {
            return await _ctx.Users.CountAsync();
        }

        public async Task<User?> FindUserByIdAsync(string id)
        {
            return await _ctx.Users.FindAsync(id);
        }

        public async Task<User?> FindUserByNameAsync(string name)
        {
            foreach (var user in await _ctx.Users.ToListAsync())
                if (user.Name.Equals(name))
                    return user;
            return null;
        }

        public bool VerifyUserPassword(string password, User user)
        {
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(user.PasswordHash);
        }

        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task<User> CreateNewUserAsync(UserDto userDto)
        {
            CreatePasswordHash(userDto.Password, out byte[] passwordSalt, out byte[] passwordHash);

            var newUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = userDto.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                LastLoginAt = DateTime.UtcNow,
                RegisteredAt = DateTime.UtcNow
            };
            _ctx.Users.Add(newUser);

            await _ctx.SaveChangesAsync();

            return newUser;
        }

        public async Task<Album> CreateNewAlbumForUserAsync(User user, AlbumDto albumDto)
        {
            var newAlbum = new Album()
            {
                Id = Guid.NewGuid().ToString(),
                Name = albumDto.Name,
                User = user,
                UserId = user.Id,
                ParentName = albumDto.ParentName,
                Description = albumDto.Description,
                CreatedAt = DateTime.UtcNow
            };
            user.Albums.Add(newAlbum);
            _ctx.Albums.Add(newAlbum);
            await _ctx.SaveChangesAsync();

            return newAlbum;
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            var user = await _ctx.Users.FindAsync(id);
            if (user == null)
                return;
            _ctx.Users.Remove(user);
            await _ctx.SaveChangesAsync();
        }
    }
}
