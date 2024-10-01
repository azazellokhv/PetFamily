using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Features.VolunteersManagement.AddPetPhoto;

public class AddPetPhotoHandler
{
    private const string BUCKET_NAME = "photos";
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetPhotoCommand> _validator;
    private readonly ILogger _logger;

    public AddPetPhotoHandler(
        IFileProvider fileProvider, 
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetPhotoCommand> validator,
        ILogger logger
        )
    {
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        var volunteer = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var pet = volunteer.Value.GetPetById(command.PetId);
        if (pet.IsFailure)
            return pet.Error.ToErrorList();

        try
        {
            List<FileContent> fileContents = [];
            foreach (var photo in command.PetPhotos)
            {
                var extension = Path.GetExtension(photo.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return filePath.Error.ToErrorList();

                var fileContent = new FileContent(
                    photo.Content, filePath.Value.Path);

                fileContents.Add(fileContent);
            }

            var fileData = new FileData(fileContents, BUCKET_NAME);

            var uploadResult = await _fileProvider.UploadFiles(fileData, cancellationToken);
            if (uploadResult.IsFailure)
                return uploadResult.Error.ToErrorList();

            var filePaths = command.PetPhotos
                .Select(p => FilePath.Create(Guid.NewGuid(), p.FileName).Value);

            var petPhotos = filePaths.Select(p => new PetPhoto(p, false));

            pet.Value.UpdatePhotos(new PetPhotoList(petPhotos));

            volunteer.Value.AddPet(pet.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            transaction.Commit();
            
            _logger.LogInformation("Success uploaded photos to pet - {id}", pet.Value.Id.Value);
            
            return pet.Value.Id.Value;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Fail to upload photos for pet {petId}", command.PetId);
            
            transaction.Rollback();
            
            return pet.Value.Id.Value;
        }
    }
}