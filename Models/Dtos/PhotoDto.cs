namespace PhotoGalleryAPI.Models.Dtos
{
    public record PhotoDto(string Name, string Description, DateTime TakenAtTime, string TakenAtLocation, string ImageData);
}
