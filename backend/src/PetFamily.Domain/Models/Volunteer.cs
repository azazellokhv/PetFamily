using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public class Volunteer
{
    private readonly List<Pet> _pets = default!;
    
    //For EF Сore
    private Volunteer()
    {
    }
    
    private Volunteer(
        Guid id, 
        FullName fullName, 
        string description, 
        int workExperience, 
        int countPetsFindHome, 
        int countPetsNeedHome, 
        int countPetsTreated, 
        string contactPhone, 
        SocialNetwork socialNetwork,
        DetailsForAssistance detailsForAssistance,
        IEnumerable<Pet> pets)
    
    {
        Id = new Guid();
        FullName = fullName;
        Description = description;
        WorkExperience = workExperience;
        CountPetsFindHome = countPetsFindHome;
        CountPetsNeedHome = countPetsNeedHome;
        CountPetsTreated = countPetsTreated;
        ContactPhone = contactPhone;
        SocialNetwork = socialNetwork;
        DetailsForAssistance = detailsForAssistance;
        
    }
    
    public Guid Id { get; private set; }
    public FullName FullName { get; private set; }
    public string Description { get; private set; }
    public int WorkExperience { get; private set; }
    public int CountPetsFindHome { get; private set; }
    public int CountPetsNeedHome { get; private set; }
    public int CountPetsTreated { get; private set; }
    public string ContactPhone { get; private set; }
    public SocialNetwork SocialNetwork { get; private set; }
    public DetailsForAssistance DetailsForAssistance { get; private set; }
    
    public IReadOnlyList<Pet> Pets => _pets;

    public void AddPat(Pet pet)
    {
        _pets.Add(pet);
    }

    public static Result<Volunteer> Create(
        Guid id, 
        FullName fullName, 
        string description, 
        int workExperience, 
        int countPetsFindHome, 
        int countPetsNeedHome, 
        int countPetsTreated, 
        string contactPhone, 
        SocialNetwork socialNetwork,
        DetailsForAssistance detailsForAssistance,
        IEnumerable<Pet> pets)
    {
        if (workExperience < 0 || workExperience > 100)
            return Result.Failure<Volunteer>("Не верно указан опыт работы"); 
        
        if (countPetsFindHome < 0)
            return Result.Failure<Volunteer>("Не верно указано количество найденых питомцев"); 
        
        if (contactPhone.Length != Pet.LENGTH_PHONE_NUMBER && IsDigitsOnly(contactPhone))
            return Result.Failure<Volunteer>("Не верно указан контактный номер телефона");
        

        var volunteer = new Volunteer(
            id, 
            fullName, 
            description, 
            workExperience, 
            countPetsFindHome, 
            countPetsNeedHome, 
            countPetsTreated, 
            contactPhone, 
            socialNetwork,
            detailsForAssistance,
            pets);

        return Result.Success(volunteer);

    }
    private static bool IsDigitsOnly(string? checkString)
    {
        if (checkString == null) 
            return false;
        
        return checkString.All(c => c >= '0' && c <= '9');
    }
}