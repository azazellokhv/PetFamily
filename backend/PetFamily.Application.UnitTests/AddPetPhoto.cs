using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;
using PetFamily.Application.Database;
using PetFamily.Application.DTOs;
using PetFamily.Application.Features.VolunteersManagement;
using PetFamily.Application.Features.VolunteersManagement.AddPetPhoto;
using PetFamily.Application.FileProvider;
using PetFamily.Domain.BiologicalSpeciesManagement.ValueObjects;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Enum;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.UnitTests;

public class AddPetPhoto
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    
    private readonly Mock<IFileProvider> _fileProviderMock = new();
    private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock = new();
    private readonly Mock<IValidator<AddPetPhotoCommand>> _validatorMock = new();
    private readonly Mock<ILogger<AddPetPhotoHandler>> _loggerMock = new();

    [Fact]
    public async Task Handle_Should_Add_Pet_Photo()
    {
        // arrange
        
        const int petsCount = 1;
        var volunteer = CreateVolunteerWithPats(petsCount);
        var cancellationToken = new CancellationTokenSource().Token;
        
        var stream = new MemoryStream();
        var fileName = "test.jpg";
        var uploadFileDto = new UploadFileDto(stream, fileName, false, "test");
        List<UploadFileDto> uploadFiles = [uploadFileDto, uploadFileDto];
        
        var command = new AddPetPhotoCommand(
            volunteer.Id.Value,
            volunteer.Pets[0].Id.Value,
            uploadFiles);
        
        List<FilePath> filePaths = [
            FilePath.Create(fileName).Value,
            FilePath.Create(fileName).Value];
        
        _fileProviderMock
            .Setup(v => v.UploadFiles(It.IsAny<List<FileData>>(), cancellationToken))
            .ReturnsAsync(Result.Success<IReadOnlyList<FilePath>, Error>(filePaths));
        
        _volunteersRepositoryMock.Setup(v => v.GetById(volunteer.Id, cancellationToken))
            .ReturnsAsync(volunteer);
        
        _unitOfWork.SaveChanges(cancellationToken).Returns(Task.CompletedTask);
        
        _validatorMock.Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(new ValidationResult());
        
        //var loggerMock = new Mock<ILogger<AddPetPhotoHandler>>();
        
        var handler = new AddPetPhotoHandler(
            _fileProviderMock.Object,
            _unitOfWork,
            _volunteersRepositoryMock.Object,
            _validatorMock.Object,
            _loggerMock.Object); 
        
        // act
        var result = await handler.Handle(command, cancellationToken);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Pets[0].Id.Value);

    }
    
    
    
    private Volunteer CreateVolunteer()
    {
        var fullName = FullName.Create("John", "Doe", "Smith").Value;
        var description = Description.Create("Test").Value;
        var workExperience = WorkExperience.Create(10).Value;
        var phoneNumber = PhoneNumber.Create("123").Value;
        IEnumerable<SocialNetwork> socialNetwork = new List<SocialNetwork>();
        var socialNetworksList = new SocialNetworksList(socialNetwork);
        IEnumerable<DetailForAssistance> volunteerDetails = new List<DetailForAssistance>();
        var volunteerDetailsList = new VolunteerDetailsList(volunteerDetails);

        var volunteerResult = Volunteer.Create(
            VolunteerId.NewVolunteerId(),
            fullName,
            description,
            workExperience,
            phoneNumber,
            socialNetworksList,
            volunteerDetailsList);

        return volunteerResult.Value;
    }

    private Pet CreatePet()
    {
        var petId = PetId.NewPetId();
        var nickName = Nickname.Create("Nickname").Value;
        var petType = PetType.Create(
            BiologicalSpeciesId.Empty(),
            BreedId.Empty()).Value;
        var description = Description.Create("Description").Value;
        var color = Color.Create("Color").Value;
        var health = Health.Create(
            true,
            "DescriptionDisease").Value;
        var address = Address.Create(
            "Country",
            "Locality",
            "Street",
            "BuildingNumber",
            "Comments").Value;
        var weight = Weight.Create("Weight").Value;
        var height = Height.Create("Height").Value;
        var phoneNumber = PhoneNumber.Create("PhoneNumber").Value;
        var detailForAssistance = DetailForAssistance.Create(
            "Title",
            "Description",
            "ContactPhoneAssistance",
            "BankCardAssistance").Value;
        var dateOfCreation = DateTime.Now.ToUniversalTime();
        var assistanceStatus = AssistanceStatus.NeedHelp;

        IEnumerable<PetPhoto> petPhotos = [];

        var resultPet = Pet.Create(
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
            true,
            Convert.ToDateTime("2020-01-01 00:00:00"),
            true,
            assistanceStatus,
            detailForAssistance,
            dateOfCreation,
            new PetPhotoList(petPhotos)).Value;

        return resultPet;
    }

    private Volunteer CreateVolunteerWithPats(int countPets)
    {
        var resultVolunteer = CreateVolunteer();

        for (var i = 0; i < countPets; i++)
        {
            var pet = CreatePet();
            resultVolunteer.AddPet(pet);
        }
        
        return resultVolunteer;
    }
}