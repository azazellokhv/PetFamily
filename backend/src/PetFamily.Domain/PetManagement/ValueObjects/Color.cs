using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Color
{
    private Color(string value)
    {
        Value = value;
    }
    public string Value { get; }
    
    public static Result<Color, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(Color));

        return new Color(value);
    }
    
}