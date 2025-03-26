using PetFamily.Application.DTOs;

namespace PetFamily.Application.Features.VolunteersManagement.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId, 
    FullNameDto FullName, 
    string Description, 
    int WorkExperience,
    string PhoneNumber);