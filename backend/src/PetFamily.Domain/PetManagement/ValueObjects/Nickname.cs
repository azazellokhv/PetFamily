using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Nickname
{
    private Nickname(string value)
    {
        Value = value;
    }
    public string Value { get; }
    
    public static Result<Nickname, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(Nickname));

        return new Nickname(value);
    }
    
}