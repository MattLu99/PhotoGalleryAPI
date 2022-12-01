# PhotoGalleryAPI
ASP.NET Core 6 backend for the PhotoGallery university project. It handles all data, including users, image's and everything that is needed for these to work.

# Usefull commands

## Build and Run:
dotnet publish<br/>
dotnet run PhotoGallery.dll

## Erase DB+Migration and start again:
dotnet ef database update 0<br/>
dotnet ef migrations remove<br/>
Add-Migration GalleryModels

# Models

## User
Users are the main and most important data of this application. Anyone can register and become a user. Users can upload images make their own albums, to save their images and keep them for later viewing.

## Album
Albums store photos for the users. Albums know what user they belond to and also know their own "location" within the user's album selection.

## Photo
Photos are the representation of a single image with all its necessery data.

# Links
- [Create README](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops)

# ToDo:
- Fix: [make lazy loader not show in any Json output](https://stackoverflow.com/questions/25749509/how-can-i-tell-json-net-to-ignore-properties-in-a-3rd-party-object)
- Feature: ensure that EF Core can take Image hash (maybe in bytes array)
- Test: images properly uploaded to database