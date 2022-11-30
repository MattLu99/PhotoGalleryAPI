using PhotoGalleryAPI.Models.Data;

namespace PhotoGalleryAPI.Models.Dto
{
    public record AlbumDto(string Name, User User, string ParentName, string Description);
}
