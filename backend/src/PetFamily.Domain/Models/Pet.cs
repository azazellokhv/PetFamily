using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public class Pet
{
    public const int MAX_NICKNAME_LENGTH = 100;
    
    //For EF core
    private Pet()
    {
    }

    private Pet(
        string nickname,
        BiologicalSpecies biologicalSpecies, 
        string description, 
        Breed breed, 
        ColorPet colorPet,
        Health health,
        Address address,
        int weight,
        int height,
        int contactPhone,
        bool isNeutered,
        DateOnly birthday,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        DetailsForAssistance detailsForAssistance)
    {
        Nickname = nickname;
        BiologicalSpecies = biologicalSpecies;
        Description = description;
        Breed = breed;
        ColorPet = colorPet;
        Health = health;
        Address = address;
        Weight = weight;
        Height = height;
        ContactPhone = contactPhone;
        IsNeutered = isNeutered;
        Birthday = birthday;
        IsVaccinated = isVaccinated;
        AssistanceStatus = assistanceStatus;
        DetailsForAssistance = detailsForAssistance;
    }
    
    public Guid Id { get; private set; }
    public string Nickname { get; private set; }
    public BiologicalSpecies BiologicalSpecies { get; private set; }
    public string Description { get; private set; } = default!;
    public Breed Breed { get; private set; }
    public ColorPet ColorPet { get; private set; }
    public Health Health { get; private set; }
    public Address Address { get; private set; }
    public int Weight { get; private set; }
    public int Height { get; private set; }
    public int ContactPhone { get; private set; }
    public bool IsNeutered { get; private set; }
    public DateOnly Birthday { get; private set; }
    public bool IsVaccinated { get; private set; }
    public AssistanceStatus AssistanceStatus { get; private set; }
    public DetailsForAssistance DetailsForAssistance { get; private set; }
    public DateTime DateOfCreation { get; private set; }


    public static Result<Pet> Create(
        string nickname,
        BiologicalSpecies biologicalSpecies,
        string description,
        Breed breed,
        ColorPet colorPet,
        Health health,
        Address address,
        int weight,
        int height,
        int contactPhone,
        bool isNeutered,
        DateOnly birthday,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        DetailsForAssistance detailsForAssistance)
    {
        if (string.IsNullOrWhiteSpace(nickname) || nickname.Length > MAX_NICKNAME_LENGTH)
            return Result.Failure<Pet>("Не указана кличка питомца");

        if (string.IsNullOrWhiteSpace(biologicalSpecies.Title))
            return Result.Failure<Pet>("Не указан вид питомца");
        
        if (string.IsNullOrWhiteSpace(address.Country) || 
            string.IsNullOrWhiteSpace(address.Locality) ||
            string.IsNullOrWhiteSpace(address.Street) ||
            string.IsNullOrWhiteSpace(address.BuildingNumber))
            return Result.Failure<Pet>("Не полностью указан адрес нахождения питомца");
        
        if (contactPhone != 11)
            return Result.Failure<Pet>("Не верно указан контактный номер телефона");
        
        if (string.IsNullOrWhiteSpace(assistanceStatus.Title))
            return Result.Failure<Pet>("Не указан статус помощи");

        var pet = new Pet(nickname, biologicalSpecies, description, breed, colorPet, health, address, weight,
            height, contactPhone, isNeutered, birthday, isVaccinated, assistanceStatus, detailsForAssistance);

        return Result.Success(pet);
    }
}
