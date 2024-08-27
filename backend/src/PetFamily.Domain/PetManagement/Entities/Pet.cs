using CSharpFunctionalExtensions;
using PetFamily.Domain.BiologicalSpeciesManagement.Entities;
using PetFamily.Domain.BiologicalSpeciesManagement.ValueObjects;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Domain.PetManagement.Entities;

public class Pet : Shared.Entity<PetId>
{
    //For EF Сore
    private Pet(PetId id) : base(id)
    {
    }

    private Pet(
        PetId petId,
        Nickname nickname,
        PetType petType,
        Description description,
        Color color,
        Health health,
        Address address,
        Weight weight,
        Height height,
        PhoneNumber phoneNumber,
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
        Color = color;
        Health = health;
        Address = address;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        IsNeutered = isNeutered;
        Birthday = birthday;
        IsVaccinated = isVaccinated;
        AssistanceStatus = assistanceStatus;
        DetailsForAssistance = detailsForAssistance;
        DateOfCreation = DateTime.UtcNow;
        PetPhotoList = petPhotoList;
    }

    public Nickname Nickname { get; private set; }
    public PetType PetType { get; private set; }
    public Description Description { get; private set; }
    public Color Color { get; private set; }
    public Health Health { get; private set; }
    public Address Address { get; private set; }
    public Weight Weight { get; private set; }
    public Height Height { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public bool IsNeutered { get; private set; }
    public DateOnly Birthday { get; private set; }
    public bool IsVaccinated { get; private set; }
    public AssistanceStatus AssistanceStatus { get; private set; }
    public DetailsForAssistance DetailsForAssistance { get; private set; }
    public DateTime DateOfCreation { get; private set; }
    public PetPhotoList? PetPhotoList { get; private set; }

    public static Result<Pet> Create(
        PetId petId,
        Nickname nickname,
        PetType petType,
        Description description,
        Breed breed,
        Color color,
        Health health,
        Address address,
        Weight weight,
        Height height,
        PhoneNumber phoneNumber,
        bool isNeutered,
        DateOnly birthday,
        bool isVaccinated,
        AssistanceStatus assistanceStatus,
        DetailsForAssistance detailsForAssistance,
        PetPhotoList petPhotoList)
    {
        return new Pet(
            petId,
            nickname,
            petType,
            description,
            color,
            health,
            address,
            weight,
            height,
            phoneNumber,
            isNeutered,
            birthday,
            isVaccinated,
            assistanceStatus,
            detailsForAssistance,
            petPhotoList);
    }
}