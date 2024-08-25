using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Description
{
    private Description(string value)
    {
        Value = value;
    }
    public string Value { get; }
    
    public static Result<Description> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Description>("Заполните дополнительную информацию");

        if (value.Length > Constants.MAX_DESCRIPTION_LENGTH)
            return Result.Failure<Description>("Превышен размер поля для дополнительной информации");
        
        var description = new Description(value);
        
        return Result.Success(description);
    }
    
}