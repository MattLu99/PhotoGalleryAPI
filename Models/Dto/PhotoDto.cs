using PhotoGalleryAPI.Models.Data;

namespace PhotoGalleryAPI.Models.Dto
{
    public record PhotoDto(string Name, Album Album, string Description, DateTime TakenAtTime, string TakenAtLocation, byte[] imageData);
}
