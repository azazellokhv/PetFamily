namespace PetFamily.Application.DTOs.Queries;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public string LastName { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string Patronymic { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int WorkExperience { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public IEnumerable<SocialNetworkDto> SocialNetworks { get; init; } = default!;
    public IEnumerable<VolunteerDetailsDto> VolunteerDetails { get; init; } = default!;
    public PetDto[] Pets { get; init; } = [];
}