using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Models.Entities;
using PhotoGalleryAPI.Models.Dtos;
using PhotoGalleryAPI.Services;
using System.Linq;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _context;

        public UserController(IUserService context)
        {
            _context = context;
        }

        // GET: api/<UserController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return Ok(await _context.GetUsersAsync());
        }

        // GET: api/<UserController>/Count
        [HttpGet("Count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<ActionResult<int>> Count()
        {
            return Ok(await _context.CountUsersAsync());
        }

        // GET: api/<UserController>/...
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _context.FindUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found!");

            return Ok(user);
        }

        // GET: api/<UserController>/.../Albums
        [HttpGet("{id}/Albums")]
        [ProducesResponseType(typeof(IEnumerable<Album>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Album>>> GetUserAlbums(string id)
        {
            var user = await _context.FindUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found!");
            List<Album> albums = user.Albums;
            return Ok(albums);
        }

        // GET: api/<UserController>/.../AlbumsByLocation/...
        [HttpGet("{id}/AlbumsByLocation/{location}")]
        [ProducesResponseType(typeof(IEnumerable<Album>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Album>>> GetUserAlbumsByLocation(string id, string location)
        {
            var user = await _context.FindUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found!");
            List<Album> albums = user.Albums.FindAll(a => a.ParentName.Equals(location));
            return Ok(albums);
        }

        // POST: api/<UserController>/Register
        [HttpPost("Register")]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<User>>> Register(UserDto request)
        {
            var user = await _context.FindUserByNameAsync(request.Name);
            if (user != null)
                return BadRequest("Username is taken!");
            var newUser = await _context.CreateNewUserAsync(request);

            return Created("api/User/" + newUser.Id, newUser);
        }

        // POST: api/<UserController>/Login
        [HttpPost("Login")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> Login(UserDto request)
        {
            var user = await _context.FindUserByNameAsync(request.Name);
            if (user == null)
                return BadRequest("Bad username!");
            if (!_context.VerifyUserPassword(request.Password, user))
                return BadRequest("Bad password!");

            user.LastLoginAt = DateTime.UtcNow;

            return Ok(user);
        }

        // POST: api/<UserController>/.../NewAlbum
        [HttpPost("{id}/NewAlbum")]
        [ProducesResponseType(typeof(Album), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Album>> NewAlbum(string id, AlbumDto request)
        {
            var user = await _context.FindUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found!");
            if (request.Name.Contains('$'))
                return BadRequest("Album name can't contain dollar sign!");
            foreach (var album in user.Albums)
                if (album.Name.Equals(request.Name))
                    return BadRequest("Album name is taken!");

            var newAlbum = await _context.CreateNewAlbumForUserAsync(user, request);
            return Created("api/Album/" + newAlbum.Id, newAlbum);
        }

        // DELETE api/<UserController>/...
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _context.FindUserByIdAsync(id);
            if (user == null)
                return NotFound("User not found!");
            await _context.DeleteUserByIdAsync(id);

            return NoContent();
        }
    }
}
