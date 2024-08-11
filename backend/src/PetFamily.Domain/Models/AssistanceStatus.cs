using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//статус помощи
public class AssistanceStatus 
{
    private AssistanceStatus(string title)
    {
        Id = new Guid();
        Title = title;
    }
    public Guid Id { get; private set; }

    public string Title { get; private set; }

    public static Result<AssistanceStatus> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<AssistanceStatus>("Не указан статус помощи");

        var assistanceStatus = new AssistanceStatus(title);

        return Result.Success(assistanceStatus);
    }

}