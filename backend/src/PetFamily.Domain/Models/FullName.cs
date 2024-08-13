using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public class FullName
{
    private FullName(string lastName, string firstName, string patronymic)
    {
        Patronymic = patronymic;
        LastName = lastName;
        FirstName = firstName;
    }
    
    public string LastName { get; private set; }
    public string FirstName { get; private set; }
    public string Patronymic { get; private set; }

    public static Result<FullName> Create(string lastName, string firstName, string patronymic)
    {
        if (lastName.Length > Pet.MAX_NAME_LENGTH ||
            firstName.Length > Pet.MAX_NAME_LENGTH ||
            patronymic.Length > Pet.MAX_NAME_LENGTH)
            return Result.Failure<FullName>("Фамилия, имя или отчество превышают предельную длину");
        
        var fullName = new FullName(lastName, firstName, patronymic);
        
        return Result.Success(fullName);
    }
    

}