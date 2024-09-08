using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName, 
    string Description, 
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<DetailsForAssistanceDto> DetailsForAssistance,
    IEnumerable<SocialNetworksDto> SocialNetworks);