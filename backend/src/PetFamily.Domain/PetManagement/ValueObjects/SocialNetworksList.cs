namespace PetFamily.Domain.PetManagement.ValueObjects;

public record SocialNetworksList
{
    private SocialNetworksList() 
    {
    }
    public SocialNetworksList(IEnumerable<SocialNetwork> socialNetwork)
    {
        SocialNetworks = socialNetwork.ToList();
    }
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
}