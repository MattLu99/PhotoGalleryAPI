using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            var rootGallery = new Gallery()
            {
                Id = Guid.NewGuid(),
                Name = "#root",
                User = newUser,
                UserId = newUser.Id,
                CreatedAt = DateTime.UtcNow
            };
            _context.Galleries.Add(rootGallery);
            newUser.Galleries.Add(_context.Galleries.OrderBy(g => g.CreatedAt).Last());

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Created("api/User/" + newUser.Id, newUser);
        }

        // POST: api/<UserController>/Login
        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<User>>> Login(UserDto request)
        {
            var user = await _context.Users.Where(user => user.Name.Equals(request.Name)).FirstOrDefaultAsync();
            if (user == default)
                return BadRequest("Bad username!");
            if (!_userService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return Ok("Bad password!");

            string token = _userService.CreateToken(user.Name);
            return Ok(token);
        }
    }
}
