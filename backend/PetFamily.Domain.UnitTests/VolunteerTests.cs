using FluentAssertions;
using PetFamily.Domain.BiologicalSpeciesManagement.ValueObjects;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Enum;
using PetFamily.Domain.Shared.Ids;

namespace UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Add_Pet_Return_Success()
    {
        // arrange
        var volunteer = CreateVolunteer();
        var pet = CreatePet();

        // act
        var result = volunteer.AddPet(pet);
        
        
        // assert
        var addedPetResult = volunteer.GetPetById(pet.Id);
        
        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(pet.Id);
        addedPetResult.Value.Position.Should().Be(Position.First);

    }
    
    [Fact]
    public void Add_Pet_With_Other_Pet_Return_Success()
    {
        // arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPats(petsCount);
        
        var petToAdd = CreatePet();
        
        // act
        var result = volunteer.AddPet(petToAdd);
        
        // assert
        var addedPetResult = volunteer.GetPetById(petToAdd.Id);
        var serialNumber = Position.Create(petsCount + 1).Value;
        
        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(petToAdd.Id);
        addedPetResult.Value.Position.Should().Be(serialNumber);
    }

    [Fact]
    public void Move_Pet_Should_Not_Move_When_Pet_Already_At_New_Position()
    {
        // arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPats(petsCount);
        
        var positionTo = Position.Create(2).Value;
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
    
        // act
        var result = volunteer.MovePet(secondPet, positionTo);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
        fourthPet.Position.Value.Should().Be(4);
        fifthPet.Position.Value.Should().Be(5);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Other_Pet_Forward_When_New_Position_Is_Lower()
    {
        // arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPats(petsCount);
        
        var positionTo = Position.Create(2).Value;
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
    
        // act
        var result = volunteer.MovePet(fourthPet, positionTo);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(2);
        fifthPet.Position.Value.Should().Be(5);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Other_Pet_Back_When_New_Position_Is_Grater()
    {
        // arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPats(petsCount);
        
        var positionTo = Position.Create(4).Value;
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
    
        // act
        var result = volunteer.MovePet(secondPet, positionTo);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(4);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void Move_Pet_Should_Move_Other_Pet_Forward_When_New_Position_Is_First()
    {
        // arrange
        const int petsCount = 5;
        var volunteer = CreateVolunteerWithPats(petsCount);
        
        var positionTo = Position.Create(1).Value;
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
    
        // act
        var result = volunteer.MovePet(fifthPet, positionTo);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(5);
        fifthPet.Position.Value.Should().Be(1);
    }
    
    [Fact]
    public void Move_Pet_Should_Move_Other_Pet_Back_When_New_Position_Is_Last()
    {
        // arrange
        const int petsCount = 6;
        var volunteer = CreateVolunteerWithPats(petsCount);
        
        var positionTo = Position.Create(5).Value;
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
    
        // act
        var result = volunteer.MovePet(firstPet, positionTo);
        
        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(5);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(4);
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