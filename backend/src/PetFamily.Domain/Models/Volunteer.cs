using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

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
        string description, 
        int workExperience, 
        string contactPhone, 
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
    public string Description { get; private set; }
    public int WorkExperience { get; private set; }
 
    public string ContactPhone { get; private set; }
    public VolunteerDetails? VolunteerDetails { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;

    public void AddPat(Pet pet) => _pets.Add(pet);

    public int CountPetsFindHome() =>
        _pets.Count(p => p.AssistanceStatus.Title == "");
    public int CountPetsNeedHome() =>
        _pets.Count(p => p.AssistanceStatus.Title == "");
    public int CountPetsTreated => 
        _pets.Count(p => p.AssistanceStatus.Title == "");
    
    public static Result<Volunteer> Create(
        VolunteerId volunteerId, 
        FullName fullName, 
        string description, 
        int workExperience, 
        string contactPhone, 
        VolunteerDetails volunteerDetails)
    {
        if (workExperience < Constants.MIN_WORK_EXPERIENCE ||
            workExperience > Constants.MAX_WORK_EXPERIENCE)
            return Result.Failure<Volunteer>("Не верно указан опыт работы"); 

        if (contactPhone.Length != Constants.LENGTH_PHONE_NUMBER && IsDigitsOnly(contactPhone))
            return Result.Failure<Volunteer>("Не верно указан контактный номер телефона");
        
        return new Volunteer(
                           volunteerId, 
                           fullName, 
                           description, 
                           workExperience, 
                           contactPhone, 
                           volunteerDetails
                           );

    }
    private static bool IsDigitsOnly(string? checkString)
    {
        if (checkString == null) 
            return false;
        
        return checkString.All(c => c >= '0' && c <= '9');
    }
}