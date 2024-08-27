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
    
    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_DESCRIPTION_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(Description));

        return new Description(value);
    }
    
}