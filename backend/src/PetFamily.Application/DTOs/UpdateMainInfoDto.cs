namespace PetFamily.Application.DTOs;

public record UpdateMainInfoDto(
    FullNameDto FullName, 
    string Description, 
    int WorkExperience,
    string PhoneNumber);