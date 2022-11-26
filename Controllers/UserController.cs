using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Data;
using PhotoGalleryAPI.Models;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PhotoGalleryDbContext _context;

        public UserController(PhotoGalleryDbContext context)
        {
            _context = context;
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // POST: api/<UserController>
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        public async Task<ActionResult<List<User>>> Register(UserDto userDto)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = userDto.Name,
                Password = userDto.Password,
                RegisteredAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Created("api/User/" + user.Id, user);
        }
    }
}
