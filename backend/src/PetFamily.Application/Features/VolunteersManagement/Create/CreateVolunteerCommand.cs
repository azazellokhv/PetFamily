using PetFamily.Application.DTOs;

namespace PetFamily.Application.Features.VolunteersManagement.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName, 
    string Description, 
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<DetailsForAssistanceDto> DetailsForAssistance,
    IEnumerable<SocialNetworksDto> SocialNetworks);