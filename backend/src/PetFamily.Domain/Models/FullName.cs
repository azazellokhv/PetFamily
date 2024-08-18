using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public class FullName
{
    private FullName(string lastName, string firstName, string patronymic)
    {
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        
    }
    
    public string LastName { get; private set; }
    public string FirstName { get; private set; }
    public string Patronymic { get; private set; }

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