# PhotoGalleryAPI
ASP.NET Core 6 backend for the PhotoGallery project. Using EntityFramework Core 6 it handles all the database management for the users, albums, and images.

## Useful commands

### Build and Run:
dotnet publish<br/>
dotnet run PhotoGallery.dll

### Erase DB+Migration and start again:
dotnet ef database update 0<br/>
dotnet ef migrations remove<br/>
dotnet ef migrations add GalleryModels

## Models

### User
Users are the main and most important data of this application. Anyone can register and become a user. Users can upload images, make their own albums and save their images to albums for later viewing.

### Album
Albums store photos for the users. Albums know what user they belong to and also know their own "location" within the user's albums.

### Photo
Photos are the representation of a single image with all its necessery data. It stores the image data as a base64 string in a nvarchar(max), which in MSSQL is capable of storing up to 2GBs of data.

## Links
- [ASP.NET Core 6](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)

## ToDo:
- Feature: user/album statistics
- Feature: update/edit entities
- Feature: production version