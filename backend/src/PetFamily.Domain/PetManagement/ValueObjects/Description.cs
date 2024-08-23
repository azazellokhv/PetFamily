using CSharpFunctionalExtensions;

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

        var description = new Description(value);
        
        return Result.Success(description);
    }
    
}