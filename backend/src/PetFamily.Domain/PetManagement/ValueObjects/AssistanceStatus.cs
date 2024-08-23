using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetManagement.ValueObjects;

//статус помощи
public record AssistanceStatus 
{
    private AssistanceStatus(string title)
    {
       Title = title;
    }
    
    public string Title { get; }
    
    public static Result<AssistanceStatus> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<AssistanceStatus>("Не указан статус помощи");

        var assistanceStatus = new AssistanceStatus(title);

        return Result.Success(assistanceStatus);
    }

}