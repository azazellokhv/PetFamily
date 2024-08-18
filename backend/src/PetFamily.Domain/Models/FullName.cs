using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

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

    public static Result<FullName> Create(string lastName, string firstName, string patronymic)
    {
        if (lastName.Length > Constants.MAX_NAME_LENGTH ||
            firstName.Length > Constants.MAX_NAME_LENGTH ||
            patronymic.Length > Constants.MAX_NAME_LENGTH)
            return Result.Failure<FullName>("Фамилия, имя или отчество превышают предельную длину");
        
        var fullName = new FullName(lastName, firstName, patronymic);
        
        return Result.Success(fullName);
    }
    

}