using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Height
{
    private Height(string value)
    {
        Value = value;
    }
    public string Value { get; }
    
    public static Result<Height, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HEIGHT)
            return Errors.General.ValueIsInvalid(nameof(Height));

        return new Height(value);
    }
}