using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

//статус помощи
public record AssistanceStatus 
{
    private AssistanceStatus(string title)
    {
       Title = title;
    }
    
    public string Title { get; }
    
    public static Result<AssistanceStatus, Error> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Errors.General.ValueIsInvalid(nameof(AssistanceStatus));

        return new AssistanceStatus(title);
    }

}