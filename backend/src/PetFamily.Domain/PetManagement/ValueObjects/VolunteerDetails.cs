namespace PetFamily.Domain.PetManagement.ValueObjects;

public record VolunteerDetails

{
    private readonly List<SocialNetwork> _socialNetworks = [];
    private readonly List<DetailsForAssistance> _detailsForAssistances = [];

    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;
    public IReadOnlyList<DetailsForAssistance> DetailsForAssistance => _detailsForAssistances;
}