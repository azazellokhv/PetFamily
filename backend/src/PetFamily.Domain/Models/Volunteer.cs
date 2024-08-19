using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public sealed class Volunteer : Shared.Entity<VolunteerId>
{

   
    private readonly List<Pet> _pets = default!;
    private readonly List<SocialNetwork> _socialNetworks = default!;
    
    //For EF Сore
    private Volunteer(VolunteerId id) : base(id)
    {
    }
    
    private Volunteer(
        VolunteerId volunteerId,
        FullName fullName, 
        string description, 
        int workExperience, 
        int countPetsFindHome, 
        int countPetsNeedHome, 
        int countPetsTreated, 
        string contactPhone, 
        VolunteerDetails volunteerDetails)
        : base(volunteerId)
    
    {
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        CountPetsFindHome = countPetsFindHome;
        CountPetsNeedHome = countPetsNeedHome;
        CountPetsTreated = countPetsTreated;
        ContactPhone = contactPhone;
        VolunteerDetails = volunteerDetails;
    }

    public FullName FullName { get; private set; }
    public string Description { get; private set; }
    public int WorkExperience { get; private set; }
    public int CountPetsFindHome { get; private set; } //будет метод
    public int CountPetsNeedHome { get; private set; } //будет метод
    public int CountPetsTreated { get; private set; } //будет метод
    public string ContactPhone { get; private set; }
    public VolunteerDetails VolunteerDetails { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;

    public void AddPat(Pet pet)
    {
        _pets.Add(pet);
    }

    public static Result<Volunteer> Create(
        VolunteerId volunteerId, 
        FullName fullName, 
        string description, 
        int workExperience, 
        int countPetsFindHome, 
        int countPetsNeedHome, 
        int countPetsTreated, 
        string contactPhone, 
        VolunteerDetails volunteerDetails)
    {
        if (workExperience < Constants.MIN_WORK_EXPERIENCE ||
            workExperience > Constants.MAX_WORK_EXPERIENCE)
            return Result.Failure<Volunteer>("Не верно указан опыт работы"); 
        
        if (countPetsFindHome < Constants.MIN_PETS_FIND_HOME)
            return Result.Failure<Volunteer>("Не верно указано количество найденых питомцев"); 
        
        if (contactPhone.Length != Constants.LENGTH_PHONE_NUMBER && IsDigitsOnly(contactPhone))
            return Result.Failure<Volunteer>("Не верно указан контактный номер телефона");
        
        return new Volunteer(
                           volunteerId, 
                           fullName, 
                           description, 
                           workExperience, 
                           countPetsFindHome, 
                           countPetsNeedHome, 
                           countPetsTreated, 
                           contactPhone, 
                           volunteerDetails);

    }
    private static bool IsDigitsOnly(string? checkString)
    {
        if (checkString == null) 
            return false;
        
        return checkString.All(c => c >= '0' && c <= '9');
    }
}