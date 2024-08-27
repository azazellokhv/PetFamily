using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record ColorPet
{
    private ColorPet(string value)
    {
        Value = value;
    }
    public string Value { get; }
    
    public static Result<ColorPet, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(ColorPet));

        return new ColorPet(value);
    }
    
}