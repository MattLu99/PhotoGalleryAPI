# PhotoGalleryAPI
ASP.NET Core 6 backend for the PhotoGallery university project. It handles all data, including users, image's and everything that is needed for these to work.

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
Users are the main and most important data of this application. Anyone can register and become a user. Users can upload images make their own albums, to save their images and keep them for later viewing.

### Album
Albums store photos for the users. Albums know what user they belong to and also know their own "location" within the user's albums.

### Photo
Photos are the representation of a single image with all its necessery data. In a nvarchar(max) it stores the image data as a base64 string, which in MSSQL is capable of storing up to 2GB's of data.

## Links
- [Create README](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops)

## ToDo:
- Fix: [make lazy loader not show in any Json output](https://stackoverflow.com/questions/25749509/how-can-i-tell-json-net-to-ignore-properties-in-a-3rd-party-object)