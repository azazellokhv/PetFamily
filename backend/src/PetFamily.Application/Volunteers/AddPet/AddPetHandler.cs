using CSharpFunctionalExtensions;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.BiologicalSpeciesManagement.ValueObjects;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.AddPet;

public class AddPetHandler
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly IVolunteersRepository _volunteersRepository;

    public AddPetHandler(
        IFileProvider fileProvider,
        IVolunteersRepository volunteersRepository)
    {
        _fileProvider = fileProvider;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var petId = PetId.NewPetId();
        var nickName = Nickname.Create(command.Nickname).Value;
        var petType = PetType.Create(
            BiologicalSpeciesId.Empty(),
            BreedId.Empty()).Value;
        var description = Description.Create(command.Description).Value;
        var color = Color.Create(command.Color).Value;
        var health = Health.Create(
            command.Health.IsHealthy,
            command.Health.DescriptionDisease).Value;
        var address = Address.Create(
            command.Address.Country,
            command.Address.Locality,
            command.Address.Street,
            command.Address.BuildingNumber,
            command.Address.Comments).Value;
        var weight = Weight.Create(command.Weight).Value;
        var height = Height.Create(command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var detailForAssistance = DetailForAssistance.Create(
            command.DetailForAssistance.Title,
            command.DetailForAssistance.Description,
            command.DetailForAssistance.ContactPhoneAssistance,
            command.DetailForAssistance.BankCardAssistance).Value;

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

        var pet = Pet.Create(
            petId,
            nickName,
            petType,
            description,
            color,
            health,
            address,
            weight,
            height,
            phoneNumber,
            command.IsNeutered,
            command.Birthday,
            command.IsVaccinated,
            command.AssistanceStatus,
            detailForAssistance,
            new PetPhotoList(petPhotos)).Value;

        volunteerResult.Value.AddPet(pet);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        return pet.Id.Value;
    }
}