using PetFamily.Application.DTOs;
using PetFamily.Application.Features.VolunteersManagement.UpdateMainInfo;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateMainInfoRequest(
    FullNameDto FullName,
    string Description,
    int WorkExperience,
    string PhoneNumber)
{
    public UpdateMainInfoCommand ToCommand(Guid id) => 
        new (id, 
            FullName, 
            Description, 
            WorkExperience, 
            PhoneNumber);
}