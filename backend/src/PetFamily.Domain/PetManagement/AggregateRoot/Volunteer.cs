using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Enum;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

public sealed class Volunteer : Shared.Entity<VolunteerId>, ISoftDeletable
{
    private bool _isDeleted = false;
    private readonly List<Pet> _pets =[];

    //For EF Сore
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    private Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Description description,
        WorkExperience workExperience,
        PhoneNumber phoneNumber,
        SocialNetworksList socialNetworkList,
        VolunteerDetailsList volunteerDetailsList)
        : base(volunteerId)

    {
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
        SocialNetworkList = socialNetworkList;
        VolunteerDetailsList = volunteerDetailsList;
    }

    public FullName FullName { get; private set; }
    public Description Description { get; private set; }
    public WorkExperience WorkExperience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public SocialNetworksList SocialNetworkList { get; private set; }
    public VolunteerDetailsList VolunteerDetailsList { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;


    public UnitResult<Error> AddPet(Pet pet)
    {
        _pets.Add(pet);

        return Result.Success<Error>();
    }
    
    public Pet? GetPetById(PetId petId) =>
        _pets.FirstOrDefault(x => x.Id == petId);

    public void UpdateMainInfo(
        FullName fullName,
        Description description,
        WorkExperience workExperience,
        PhoneNumber phoneNumber)
    {
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        PhoneNumber = phoneNumber;
    }

    public void Delete()
    {
        if (_isDeleted == false)
            _isDeleted = true;
    }

    public void Restore()
    {
        if (_isDeleted == true)
            _isDeleted = false;
    }

    public int CountPetsNeedHelp() =>
        _pets.Count(p => p.AssistanceStatus == AssistanceStatus.NeedHelp);

    public int CountPetsFindHome() =>
        _pets.Count(p => p.AssistanceStatus == AssistanceStatus.FindHome);

    public int CountPetsTreated =>
        _pets.Count(p => p.AssistanceStatus == AssistanceStatus.FoundHome);

    public static Result<Volunteer, Error> Create(
        VolunteerId volunteerId,
        FullName fullName,
        Description description,
        WorkExperience workExperience,
        PhoneNumber phoneNumber,
        SocialNetworksList socialNetworkList,
        VolunteerDetailsList volunteerDetailsList)
    {
        return new Volunteer(
            volunteerId,
            fullName,
            description,
            workExperience,
            phoneNumber,
            socialNetworkList,
            volunteerDetailsList);
    }
}