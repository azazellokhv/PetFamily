using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.AddPetPhoto;

public class AddPetPhotoHandler
{
    /*private readonly IFileProvider _fileProvider;
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
        return await _fileProvider.UploadFiles(
            command,
            cancellationToken);
    }*/
}