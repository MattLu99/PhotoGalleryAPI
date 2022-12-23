using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using PhotoGalleryAPI.Models.Entities;
using PhotoGalleryAPI.Models.Dtos;
using PhotoGalleryAPI.Services;

namespace PhotoGalleryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _context;

        public AlbumController(IAlbumService context)
        {
            _context = context;
        }

        // GET: api/<AlbumController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Album>), 200)]
        public async Task<ActionResult<List<Album>>> GetAll()
        {
            return Ok(await _context.GetAlbumsAsync());
        }

        // GET: api/<AlbumController>/Count
        [HttpGet("Count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<ActionResult<int>> Count()
        {
            return Ok(await _context.CountAlbumsAsync());
        }

        // GET: api/<AlbumController>/...
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Album), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Album>> GetAlbumById(string id)
        {
            var album = await _context.FindAlbumByIdAsync(id);
            if (album == null)
                return NotFound("Album not found!");

            return Ok(album);
        }

        // GET: api/<AlbumController>/.../Photos
        [HttpGet("{id}/Photos")]
        [ProducesResponseType(typeof(IEnumerable<Photo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Photo>>> GetUserAlbumPhotos(string id)
        {
            var album = await _context.FindAlbumByIdAsync(id);
            if (album == null)
                return NotFound("Album not found!");

            List<Photo> photos = album.Photos;
            return Ok(photos);
        }

        // POST: api/<AlbumController>/.../NewPhoto
        [HttpPost("{id}/NewPhoto")]
        [ProducesResponseType(typeof(Photo), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Photo>> NewPhoto(String id, PhotoDto request)
        {
            var album = await _context.FindAlbumByIdAsync(id);
            if (album == null)
                return NotFound("Album not found!");
            foreach (var photo in album.Photos)
                if (photo.Name.Equals(request.Name))
                    return BadRequest("Photo name is taken!");

            var newPhoto = await _context.CreateNewPhotoInAlbum(album, request);
            return Created("api/Photo/" + newPhoto.Id, newPhoto);
        }

        // GET: api/<AlbumController>/.../CoverImage
        [HttpGet("{id}/CoverImage")]
        [ProducesResponseType(typeof(Photo), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Photo>> GetCoverImage(string id)
        {
            var album = await _context.FindAlbumByIdAsync(id);
            if (album == null)
                return NotFound("Album not found!");
            if (album.CoverImageId.IsNullOrEmpty())
                return Ok();
            var photo = album.Photos.FindLast(p => p.Id.Equals(album.CoverImageId));
            if (photo == null)
                return NotFound("Photo not found!");
            return Ok(photo);
        }

        // PUT: api/<AlbumController>/.../ChangeCoverImage
        [HttpPut("{id}/ChangeCoverImage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> ChangeCoverImage(string id, string coverImageId)
        {
            var album = await _context.FindAlbumByIdAsync(id);
            if (album == null)
                return NotFound("Album not found!");
            var photo = await _context.ChangeCoverImagebyIdAsync(album, coverImageId);
            if (photo == null)
                return NotFound("Photo not found!");
            return Ok();
        }

        // DELETE api/<AlbumController>/...
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAlbum(string id)
        {
            var album = await _context.FindAlbumByIdAsync(id);
            if (album == null)
                return NotFound("Album not found!");

            await _context.DeleteAlbumByIdAsync(id);

            return NoContent();
        }
    }
}
