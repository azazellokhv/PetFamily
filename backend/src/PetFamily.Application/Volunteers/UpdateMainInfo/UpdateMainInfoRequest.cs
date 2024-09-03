using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoRequest(
    Guid VolunteerId, 
    UpdateMainInfoDto Dto);
    
public record FullNameDto(string LastName, string FirstName, string Patronymic);

public record UpdateMainInfoDto(
    FullNameDto FullName, 
    string Description, 
    int WorkExperience,
    string PhoneNumber);