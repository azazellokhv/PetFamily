using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Weight
{
    private Weight(string value)
    {
        Value = value;
    }
    public string Value { get; }
    
    public static Result<Weight, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_WEIGHT)
            return Errors.General.ValueIsInvalid(nameof(Weight));

        return new Weight(value);
    }
}