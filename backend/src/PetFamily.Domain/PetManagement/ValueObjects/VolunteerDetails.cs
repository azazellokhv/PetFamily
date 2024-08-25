namespace PetFamily.Domain.PetManagement.ValueObjects;

public record VolunteerDetails
{
    private VolunteerDetails () 
    {
    }
    public VolunteerDetails(
        IEnumerable<SocialNetwork> socialNetworks, 
        IEnumerable<DetailsForAssistance> detailsForAssistance)
    {
        SocialNetworks = socialNetworks.ToList();
        DetailsForAssistance = detailsForAssistance.ToList();
    }
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
    public IReadOnlyList<DetailsForAssistance> DetailsForAssistance { get; }
}