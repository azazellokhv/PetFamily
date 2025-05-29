using PetFamily.Application.Abstraction;
using PetFamily.Application.DTOs;

namespace PetFamily.Application.VolunteersManagement.Features.Create;

public record CreateVolunteerCommand(
    FullNameDto FullName, 
    string Description, 
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<DetailsForAssistanceDto> DetailsForAssistance,
    IEnumerable<SocialNetworksDto> SocialNetworks) : ICommand;