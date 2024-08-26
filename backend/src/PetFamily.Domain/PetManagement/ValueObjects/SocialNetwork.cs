using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record SocialNetwork
{
    private SocialNetwork(string title, string link)
    {
        Title = title;
        Link = link;
    }
    public string Title { get; }
    public string Link { get; }

    public static Result<SocialNetwork, Error> Create(string title, string link)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Errors.General.ValueIsInvalid(nameof(title));
        
        if (string.IsNullOrWhiteSpace(link))
            return Errors.General.ValueIsInvalid(nameof(link));
     
        return new SocialNetwork(title, link);
    }

}