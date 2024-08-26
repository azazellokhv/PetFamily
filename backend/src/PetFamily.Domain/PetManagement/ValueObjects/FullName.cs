using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record FullName
{
    private FullName(string lastName, string firstName, string patronymic)
    {
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
    }

    public string LastName { get; }
    public string FirstName { get; }
    public string Patronymic { get; }

    public static Result<FullName, Error> Create(string lastName, string firstName, string? patronymic)
    {
        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(lastName));

        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(firstName));

        if (string.IsNullOrWhiteSpace(patronymic) || patronymic.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(patronymic));

        return new FullName(lastName, firstName, patronymic);
    }
}