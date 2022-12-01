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
        private readonly IAuthService _authService;

        public UserController(PhotoGalleryDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
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
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found!");

            return Ok(user);
        }

        // GET: api/<UserController>/5/Albums
        [HttpGet("{id}/Albums")]
        [ProducesResponseType(typeof(IEnumerable<Album>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Album>>> GetUserAlbums(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found!");
            List<Album> albums = user.Albums;
            return Ok(albums);
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

            _authService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
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

        // POST: api/<UserController>/Login
        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var user = await _context.Users.Where(user => user.Name.Equals(request.Name)).FirstOrDefaultAsync();
            if (user == default)
                return BadRequest("Bad username!");
            if (!_authService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Bad password!");

            user.LastLoginAt = DateTime.UtcNow;
            string token = _authService.CreateToken(user.Name);
            return Ok(token);
        }

        // POST: api/<UserController>/5/NewAlbum
        [HttpPost("{id}/NewAlbum")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Album>> NewAlbum(Guid id, AlbumDto request)
        {
            var user = await _context.Users.FindAsync(id);
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
                UserId = id,
                ParentName = request.ParentName,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };
            user.Albums.Add(newAlbum);

            _context.Albums.Add(newAlbum);
            await _context.SaveChangesAsync();
            return Created("api/Album/" + newAlbum.Id, newAlbum);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found!");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
