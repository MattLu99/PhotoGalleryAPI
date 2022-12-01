using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Data;
using PhotoGalleryAPI.Models.Data;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly PhotoGalleryDbContext _context;

        public PhotoController(PhotoGalleryDbContext context)
        {
            _context = context;
        }

        // GET: api/<PhotoController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        public async Task<ActionResult<List<Photo>>> GetAll()
        {
            return Ok(await _context.Photos.ToListAsync());
        }

        // GET: api/<PhotoController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Photo>>> GetPhotoById(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
                return NotFound("Photo not found!");
            //Todo: debug
            Console.WriteLine("Photo: " + photo.ToString());
            //Todo: debug

            return Ok(photo);
        }

        // DELETE api/<PhotoController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeletePhoto(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
                return NotFound("Photo not found!");

            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
