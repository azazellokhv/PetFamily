namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record CreateVolunteerRequest(
    string LastName, 
    string FirstName, 
    string Patronymic,
    string Description, 
    int WorkExperience,
    string ContactPhone);