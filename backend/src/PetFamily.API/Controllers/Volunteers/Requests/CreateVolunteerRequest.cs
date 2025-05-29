using PetFamily.Application.DTOs;
using PetFamily.Application.VolunteersManagement.Features.Create;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Description,
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<DetailsForAssistanceDto> DetailsForAssistance,
    IEnumerable<SocialNetworksDto> SocialNetworks)
{
    public CreateVolunteerCommand ToCommand() =>
        new (FullName, 
            Description, 
            WorkExperience, 
            PhoneNumber, 
            DetailsForAssistance, 
            SocialNetworks);
}