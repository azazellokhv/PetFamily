using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.AddPetPhoto;

public class AddPetPhotoHandler
{
    private const string BUCKET_NAME = "photos";
    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;

    public AddPetPhotoHandler(
        IFileProvider fileProvider, 
        IVolunteersRepository volunteersRepository)
    {
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<string, Error>> Handle(
        AddPetPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error;
        
        var petId = PetId.Create(command.PetId);
        var pet = volunteer.Value.GetPetById(petId);
        if (pet is null)
            return Errors.General.NotFound(petId);
        
        List<FileContent> fileContents = [];
        foreach (var photo in command.PetPhotos)
        {
            var extension = Path.GetExtension(photo.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error;

            var fileContent = new FileContent(
                photo.Content, filePath.Value.Path);

            fileContents.Add(fileContent);
        }

        var fileData = new FileData(fileContents, BUCKET_NAME);

        var uploadResult = await _fileProvider.UploadFiles(fileData, cancellationToken);
        if (uploadResult.IsFailure)
            return uploadResult.Error;

        var filePaths = command.PetPhotos
            .Select(p => FilePath.Create(Guid.NewGuid(), p.FileName).Value);

        var petPhotos = filePaths.Select(p => new PetPhoto(p, false));
        
        pet.UpdatePhotos(new PetPhotoList(petPhotos));

        volunteer.Value.AddPet(pet);

        await _volunteersRepository.Save(volunteer.Value, cancellationToken);

        return petId.Value.ToString();
    }
}