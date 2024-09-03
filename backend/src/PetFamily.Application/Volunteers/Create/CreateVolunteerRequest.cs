using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.Create;

public record CreateVolunteerRequest(
    FullNameDto FullName, 
    string Description, 
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<DetailsForAssistanceDTO> DetailsForAssistance,
    IEnumerable<SocialNetworksDTO> SocialNetworks);
public record FullNameDto(string LastName, string FirstName, string Patronymic);