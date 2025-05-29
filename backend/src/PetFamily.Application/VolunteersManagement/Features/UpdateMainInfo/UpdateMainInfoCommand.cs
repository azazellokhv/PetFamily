using PetFamily.Application.Abstraction;
using PetFamily.Application.DTOs;

namespace PetFamily.Application.VolunteersManagement.Features.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId, 
    FullNameDto FullName, 
    string Description, 
    int WorkExperience,
    string PhoneNumber) : ICommand;