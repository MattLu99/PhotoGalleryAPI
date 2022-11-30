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
    public class UserController : ControllerBase
    {
        private readonly PhotoGalleryDbContext _context;
        private readonly IUserService _userService;

        public UserController(PhotoGalleryDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: api/<UserController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");

            return Ok(user);
        }

        // GET: api/<UserController>/5/Albums
        [HttpGet("{id}/Albums")]
        [ProducesResponseType(typeof(IEnumerable<Album>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Album>>> GetUserAlbums(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");

            return Ok(user.Albums);
        }

        // GET: api/<UserController>/5/Photos
        [HttpGet("{albumId}/Photos")]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Photo>>> GetUserAlbumPhotos(Guid userId, Guid albumId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found!");
            var album = new Album();
            Console.WriteLine(album);
            foreach (var item in user.Albums)
            {
                if (item.Id.Equals(albumId))
                    album = item;
            }
            if (album == null)
                return NotFound("Album not found!");

            return Ok(album.Photos);
        }

        // POST: api/<UserController>/Register
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

            var newGallery = new Album()
            {
                Id = Guid.NewGuid(),
                Name = "First Album",
                User = newUser,
                UserId = newUser.Id,
                ParentName = "root",
                Description = $"First album of {newUser.Name}.",
                CreatedAt = DateTime.UtcNow
            };
            newUser.Albums.Add(newGallery);
            _context.Users.Add(newUser);
            _context.Albums.Add(newGallery);
            await _context.SaveChangesAsync();
            return Created("api/User/" + newUser.Id, newUser);
        }

        // POST: api/<UserController>/Login
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
    }
}
