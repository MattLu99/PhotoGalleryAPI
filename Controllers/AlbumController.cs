using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.Data;
using PhotoGalleryAPI.Models.Data;
using PhotoGalleryAPI.Models.Dto;
using System.Collections.Generic;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly PhotoGalleryDbContext _context;

        public AlbumController(PhotoGalleryDbContext context)
        {
            _context = context;
        }

        // GET: api/<AlbumController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Album>), 200)]
        public async Task<ActionResult<List<Album>>> GetAll()
        {
            return Ok(await _context.Albums.ToListAsync());
        }

        // GET: api/<AlbumController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Album), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Album>> GetAlbumById(string id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
                return NotFound("Album not found!");

            return Ok(album);
        }

        // GET: api/<AlbumController>/5/Photos
        [HttpGet("{id}/Photos")]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Photo>>> GetUserAlbumPhotos(string id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
                return NotFound("Album not found!");

            List<Photo> photos = album.Photos;
            return Ok(photos);
        }

        // POST: api/<AlbumController>/5/NewPhoto
        [HttpPost("{id}/NewPhoto")]
        [ProducesResponseType(typeof(Photo), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Photo>> NewPhoto(String id, PhotoDto request)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
                return NotFound("Album not found!");
            foreach (var photo in album.Photos)
            {
                if (photo.Name.Equals(request.Name))
                    return BadRequest("Photo name is taken!");
            }

            var newPhoto = new Photo()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Album = album,
                AlbumId = id,
                Description = request.Description,
                TakenAtTime = request.TakenAtTime,
                TakenAtLocation = request.TakenAtLocation,
                ImageData = request.ImageData,
                UploadedAt = DateTime.Now
            };
            album.Photos.Add(newPhoto);

            _context.Photos.Add(newPhoto);
            await _context.SaveChangesAsync();
            return Created("api/Photo/" + newPhoto.Id, newPhoto);
        }

        // DELETE api/<AlbumController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAlbum(Guid id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
                return NotFound("Album not found!");

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
