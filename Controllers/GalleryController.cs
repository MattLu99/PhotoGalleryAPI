using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Data;
using PhotoGalleryAPI.Models.Data;
using PhotoGalleryAPI.Models.Dto;
using PhotoGalleryAPI.Services;
using System.Linq;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly PhotoGalleryDbContext _context;
        private readonly IUserService _userService;

        public GalleryController(PhotoGalleryDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/<GalleryController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: api/<GalleryController>/5
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUserById(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");

            return Ok(user);
        }

        // GET: api/<GalleryController>/5/Albums
        [HttpGet("{userId}/Albums")]
        [ProducesResponseType(typeof(IEnumerable<Album>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Album>>> GetUserAlbums(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");

            return Ok(user.Albums);
        }

        // GET: api/<GalleryController>/#/5
        [HttpGet("#/{albumId}")]
        [ProducesResponseType(typeof(Album), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Album>> GetAlbumById(Guid userId, Guid albumId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");

            var album = new Album();
            //Todo: debug
            Console.WriteLine(album);
            //Todo: debug
            foreach (var item in user.Albums)
            {
                if (item.Id.Equals(albumId))
                    album = item;
            }
            if (album == null)
                return NotFound("Album not found!");

            return Ok(album);
        }

        // GET: api/<GalleryController>/#/5/Photos
        [HttpGet("#/{albumId}/Photos")]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Photo>>> GetUserAlbumPhotos(Guid userId, Guid albumId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");
            var album = new Album();
            //Todo: debug
            Console.WriteLine(album);
            //Todo: debug
            foreach (var item in user.Albums)
            {
                if (item.Id.Equals(albumId))
                {
                    album = item;
                    break;
                }
            }
            if (album == null)
                return NotFound("Album not found!");

            return Ok(album.Photos);
        }

        // GET: api/<GalleryController>/#/#/5
        [HttpGet("#/#/{photoId}")]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Photo>>> GetPhotoByID(Guid albumId, Guid photoId)
        {
            var album = await _context.Albums.FindAsync(albumId);
            if (album == null)
                return NotFound("Album not found!");
            var photo = new Photo();
            //Todo: debug
            Console.WriteLine(photo);
            //Todo: debug
            foreach (var item in album.Photos)
            {
                if (item.Id.Equals(photoId))
                {
                    photo = item;
                    break;
                }
            }
            if (photo == null)
                return NotFound("Photo not found!");

            return Ok(photo);
        }

        // POST: api/<GalleryController>/Register
        [HttpPost("Register")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<User>>> Register(UserDto request)
        {
            foreach (var user in _context.Users)
            {
                if (user.Name.Equals(request.Name))
                    return BadRequest("Username is taken!");
            }

            _userService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var newUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RegisteredAt = DateTime.UtcNow
            };

            var newAlbum = new Album()
            {
                Id = Guid.NewGuid(),
                Name = "First Album",
                User = newUser,
                UserId = newUser.Id,
                ParentName = "root",
                Description = $"First album of {newUser.Name}.",
                CreatedAt = DateTime.UtcNow
            };
            newUser.Albums.Add(newAlbum);

            _context.Users.Add(newUser);
            _context.Albums.Add(newAlbum);
            await _context.SaveChangesAsync();
            return Created("api/User/" + newUser.Id, newUser);
        }

        // POST: api/<GalleryController>/Login
        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var user = await _context.Users.Where(user => user.Name.Equals(request.Name)).FirstOrDefaultAsync();
            if (user == default)
                return BadRequest("Bad username!");
            if (!_userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Bad password!");

            user.LastLoginAt = DateTime.UtcNow;
            string token = _userService.CreateToken(user.Name);
            return Ok(token);
        }

        // POST: api/<GalleryController>/5/NewAlbum
        [HttpPost("{userId}/NewAlbum")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Album>> NewAlbum(Guid userId, AlbumDto request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");
            foreach (var album in user.Albums)
            {
                if (album.Name.Equals(request.Name))
                    return BadRequest("Album name is taken!");
            }

            var newAlbum = new Album()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                User = user,
                UserId = userId,
                ParentName = request.ParentName,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };
            user.Albums.Add(newAlbum);

            _context.Albums.Add(newAlbum);
            await _context.SaveChangesAsync();
            return Created("api/User/#/" + newAlbum.Id, newAlbum);
        }

        // POST: api/<GalleryController>/#/5/NewPhoto
        [HttpPost("#/{albumId}/NewPhoto")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Album>> NewPhoto(Guid albumId, PhotoDto request)
        {
            var album = await _context.Albums.FindAsync(albumId);
            if (album == null)
                return NotFound("Album not found!");
            foreach (var photo in album.Photos)
            {
                if (photo.Name.Equals(request.Name))
                    return BadRequest("Photo name is taken!");
            }

            var newPhoto = new Photo()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Album = request.Album,
                AlbumId = albumId,
                Description = request.Description,
                TakenAtTime = request.TakenAtTime,
                TakenAtLocation = request.TakenAtLocation,
                ImageData = request.imageData,
                UploadedAt = DateTime.Now
            };
            album.Photos.Add(newPhoto);

            _context.Photos.Add(newPhoto);
            await _context.SaveChangesAsync();
            return Created("api/User/#/#/" + newPhoto.Id, newPhoto);
        }

        // DELETE api/<GalleryController>/5
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/<GalleryController>/#/5
        [HttpDelete("#/{albumId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAlbum(Guid albumId)
        {
            var album = await _context.Albums.FindAsync(albumId);
            if (album == null)
                return NotFound("Album not found!");

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/<GalleryController>/#/#/5
        [HttpDelete("#/#/{photoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePhoto(Guid photoId)
        {
            var photo = await _context.Photos.FindAsync(photoId);
            if (photo == null)
                return NotFound("Photo not found!");

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
