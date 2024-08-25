using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record ContactPhone
{
    private ContactPhone(string value)
    {
        Value = value;
    }

    public string Value { get; }
    
    public static Result<ContactPhone> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<ContactPhone>("Заполните номер телефона");

        if (value.Length != Constants.LENGTH_PHONE_NUMBER && IsDigitsOnly(value))
            return Result.Failure<ContactPhone>("Не верно указан контактный номер телефона");
        
        var contactPhone = new ContactPhone(value);
        
        return Result.Success(contactPhone);   
    }
    
    private static bool IsDigitsOnly(string? checkString)
    {
        if (checkString == null)
            return false;

        return checkString.All(c => c >= '0' && c <= '9');
    }
}