using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Domain.PetManagement.AggregateRoot;

public sealed class Volunteer : Shared.Entity<VolunteerId>
{
    private readonly List<Pet> _pets = default!;

    //For EF Сore
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    private Volunteer(
        VolunteerId volunteerId,
        FullName fullName,
        Description description,
        WorkExperience workExperience,
        ContactPhone contactPhone,
        VolunteerDetails? volunteerDetails)
        : base(volunteerId)

    {
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        ContactPhone = contactPhone;
        VolunteerDetails = volunteerDetails;
    }

    public FullName FullName { get; private set; }
    public Description Description { get; private set; }
    public WorkExperience WorkExperience { get; private set; }
    public ContactPhone ContactPhone { get; private set; }
    public VolunteerDetails? VolunteerDetails { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;

    public UnitResult<Error> AddPat(Pet pet)
    {
        _pets.Add(pet);
        
        return Result.Success<Error>();
    }


    public int CountPetsFindHome() =>
        _pets.Count(p => p.AssistanceStatus.Title == "");
    public int CountPetsNeedHome() =>
        _pets.Count(p => p.AssistanceStatus.Title == "");
    public int CountPetsTreated =>
        _pets.Count(p => p.AssistanceStatus.Title == "");

    public static Result<Volunteer, Error> Create(
        VolunteerId volunteerId,
        FullName fullName,
        Description description,
        WorkExperience workExperience,
        ContactPhone contactPhone,
        VolunteerDetails volunteerDetails)
    {
        return new Volunteer(
            volunteerId,
            fullName,
            description,
            workExperience,
            contactPhone,
            volunteerDetails);
    }

    
}