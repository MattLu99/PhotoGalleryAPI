# PhotoGalleryAPI
The backend of an Angular + ASP.NET Core web app project. Started as a university project, but then overtime worked more and more on it to learn about Fullstack development.
The backend uses ASP.NET Core for the API features and Entity Framework Core for database management.
Frontend of the project: https://github.com/MattLu99/PhotoGallery

## Useful commands

### Build and Run:
dotnet publish<br/>
dotnet run PhotoGallery.dll

### Erase DB+Migration and start again:
dotnet ef database update 0<br/>
dotnet ef migrations remove<br/>
dotnet ef migrations add GalleryModels<br/>
dotnet ef --help

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
- Update: update project to .NET 7
- Feature: complete user statistics with albums and photos
- Feature: make a seeder that can fill the database with some default data in development mode
- Feature: production version
- Fix: move all data validation and handling into the services
- Fix: make the endpoints only receive and awnser with DTOs
- Fix: have the services properly respond with their status, and have the endpoints give proper response values based on failure.