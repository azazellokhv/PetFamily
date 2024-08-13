using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public class SocialNetwork
{
    private SocialNetwork(string title, string link)
    {
        Id = new Guid();
        Title = title;
        Link = link;
    }
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Link { get; private set; }

    public static Result<SocialNetwork> Create(string title, string link)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(link))
            return Result.Failure<SocialNetwork>("Не указано название сети или ссылка сеть");
        
        var socialNetwork = new SocialNetwork(title, link);
        
        return Result.Success(socialNetwork);
    }

}