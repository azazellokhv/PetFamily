using CSharpFunctionalExtensions;

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

    public static Result<SocialNetwork> Create(string title, string link)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(link))
            return Result.Failure<SocialNetwork>("Не указано название сети или ссылка сеть");
     
        return new SocialNetwork(title, link);
    }

}