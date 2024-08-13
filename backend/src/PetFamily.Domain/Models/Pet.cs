using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public class Pet
{
    public const int MAX_NAME_LENGTH = 100;
    public const int LENGTH_PHONE_NUMBER = 100;
    
    //For EF Сore
    private Pet()
    {
    }

    private Pet(
        string nickname,
        BiologicalSpecies biologicalSpecies, 
        string description, 
        ColorPet colorPet,
        Health health,
        Address address,
        int weight,
        int height,
        string contactPhone,
        bool isNeutered,
        DateOnly birthday,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        DetailsForAssistance detailsForAssistance)
    {
        Id = new Guid();
        Nickname = nickname;
        BiologicalSpecies = biologicalSpecies;
        Description = description;
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
        DateOfCreation = DateTime.UtcNow;
    }
    
    public Guid Id { get; private set; }
    public string Nickname { get; private set; }
    public BiologicalSpecies BiologicalSpecies { get; private set; }
    public string Description { get; private set; } = default!;
    public ColorPet ColorPet { get; private set; }
    public Health Health { get; private set; }
    public Address Address { get; private set; }
    public int Weight { get; private set; }
    public int Height { get; private set; }
    public string ContactPhone { get; private set; }
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
        string contactPhone,
        bool isNeutered,
        DateOnly birthday,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        DetailsForAssistance detailsForAssistance)
    {
        if (string.IsNullOrWhiteSpace(nickname) || nickname.Length > MAX_NAME_LENGTH)
            return Result.Failure<Pet>("Не указана кличка питомца");
        
        if (contactPhone.Length != LENGTH_PHONE_NUMBER && IsDigitsOnly(contactPhone))
            return Result.Failure<Pet>("Не верно указан контактный номер телефона");

        var pet = new Pet(nickname, biologicalSpecies, description, colorPet, health, address, weight,
            height, contactPhone, isNeutered, birthday, isVaccinated, assistanceStatus, detailsForAssistance);

        return Result.Success(pet);
    }
    private static bool IsDigitsOnly(string? checkString)
    {
        if (checkString == null) 
            return false;
        
        return checkString.All(c => c >= '0' && c <= '9');
    }
}
