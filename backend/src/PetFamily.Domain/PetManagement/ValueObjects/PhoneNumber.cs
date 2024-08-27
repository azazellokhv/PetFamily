using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }
    
    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsInvalid(nameof(PhoneNumber));

        if (value.Length != Constants.LENGTH_PHONE_NUMBER && IsDigitsOnly(value))
            return Errors.General.ValueIsInvalid(nameof(PhoneNumber));
  
        return new PhoneNumber(value);   
    }
    
    private static bool IsDigitsOnly(string? checkString)
    {
        if (checkString == null)
            return false;

        return checkString.All(c => c >= '0' && c <= '9');
    }
}