using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Data;
using PhotoGalleryAPI.Models.Entities;
using PhotoGalleryAPI.Services;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _context;

        public PhotoController(IPhotoService context)
        {
            _context = context;
        }

        // GET: api/<PhotoController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        public async Task<ActionResult<List<Photo>>> GetAll()
        {
            return Ok(await _context.GetPhotosAsync());
        }

        // GET: api/<PhotoController>/Count
        [HttpGet("Count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<ActionResult<int>> Count()
        {
            return Ok(await _context.CountPhotosAsync());
        }

        // GET: api/<PhotoController>/...
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Photo>>> GetPhotoById(string id)
        {
            var photo = await _context.FindPhotoByIdAsync(id);
            if (photo == null)
                return NotFound("Photo not found!");

            return Ok(photo);
        }

        // DELETE api/<PhotoController>/...
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePhoto(string id)
        {
            var photo = await _context.FindPhotoByIdAsync(id);
            if (photo == null)
                return NotFound("Photo not found!");

            await _context.DeletePhotoByIdAsync(id);
            return NoContent();
        }
    }
}
