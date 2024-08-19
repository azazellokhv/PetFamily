using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class Pet : Shared.Entity<PetId>
{

    //For EF Сore
    private Pet (PetId id) : base(id)
    {
    }

    private Pet(
        PetId petId,
        string nickname,
        PetType petType, 
        string description, 
        string colorPet,
        Health health,
        Address address,
        int weight,
        int height,
        string contactPhone,
        bool isNeutered,
        DateOnly birthday,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        DetailsForAssistance detailsForAssistance,
        PetPhotoList petPhotoList)
        : base(petId)
    {
        Nickname = nickname;
        PetType = petType;
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
        PetPhotoList = petPhotoList;
    }

    public string Nickname { get; private set; }
    public PetType PetType { get; private set; }
    public string Description { get; private set; } = default!;
    public string ColorPet { get; private set; } = default!;
    public Health Health { get; private set; } = default!;
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
    public PetPhotoList? PetPhotoList { get; private set; }

    public static Result<Pet> Create(
        PetId petId,
        string nickname,
        PetType petType,
        string description,
        Breed breed,
        string colorPet,
        Health health,
        Address address,
        int weight,
        int height,
        string contactPhone,
        bool isNeutered,
        DateOnly birthday,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        DetailsForAssistance detailsForAssistance,
        PetPhotoList petPhotoList)
    {
        if (string.IsNullOrWhiteSpace(nickname) || nickname.Length > Constants.MAX_NAME_LENGTH)
            return Result.Failure<Pet>("Не указана кличка питомца");
        
        if (contactPhone.Length != Constants.LENGTH_PHONE_NUMBER && IsDigitsOnly(contactPhone))
            return Result.Failure<Pet>("Не верно указан контактный номер телефона");

        return new Pet(
            petId, 
            nickname, 
            petType, 
            description, 
            colorPet, 
            health, 
            address, 
            weight, 
            height, 
            contactPhone, 
            isNeutered, 
            birthday, 
            isVaccinated, 
            assistanceStatus, 
            detailsForAssistance,
            petPhotoList);
    }
    private static bool IsDigitsOnly(string? checkString)
    {
        if (checkString == null) 
            return false;
        
        return checkString.All(c => c >= '0' && c <= '9');
    }
}
