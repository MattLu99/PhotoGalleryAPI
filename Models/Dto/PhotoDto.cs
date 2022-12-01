namespace PhotoGalleryAPI.Models.Dto
{
    public record PhotoDto(string Name, string Description, DateTime TakenAtTime, string TakenAtLocation, string ImageData);
}
