using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Application.Files;
using PetFamily.Application.Messaging;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Application.VolunteersManagement.Features.AddPetPhoto;

public class AddPetPhotoHandler : ICommandHandler<Guid, AddPetPhotoCommand>
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<AddPetPhotoCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly ILogger<AddPetPhotoHandler> _logger;

    public AddPetPhotoHandler(
        IFileProvider fileProvider,
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        IValidator<AddPetPhotoCommand> validator,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        ILogger<AddPetPhotoHandler> logger
    )
    {
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _messageQueue = messageQueue;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        var volunteer = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var petResult = volunteer.Value.GetPetById(command.PetId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        try
        {
            List<FileData> filesData = [];
            foreach (var photo in command.PetPhotos)
            {
                var extension = Path.GetExtension(photo.FileName);

                var filePath = FilePath.Create(Guid.NewGuid(), extension);
                if (filePath.IsFailure)
                    return filePath.Error.ToErrorList();

                var fileData = new FileData(photo.Content, new FileInfo(filePath.Value, BUCKET_NAME));

                filesData.Add(fileData);
            }

            var filePathsResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
            if (filePathsResult.IsFailure)
            {
                await _messageQueue.WriteAsync(filesData.Select(p => p.FileInfo), cancellationToken);
                
                return filePathsResult.Error.ToErrorList();
            }

            var petPhotos = filePathsResult.Value
                .Select(p => new PetPhoto(p, false))
                .ToList();
            
            petResult.Value.UpdatePhotos(new ValueObjectList<PetPhoto>(petPhotos));

            volunteer.Value.AddPet(petResult.Value);

            await _unitOfWork.SaveChanges(cancellationToken);

            transaction.Commit();

            _logger.LogInformation("Success uploaded photos to pet - {id}", petResult.Value.Id.Value);

            return petResult.Value.Id.Value;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail to upload photos for pet {petId}", command.PetId);

            transaction.Rollback();

            return petResult.Value.Id.Value;
        }
    }
}