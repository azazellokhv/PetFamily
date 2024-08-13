using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//порода питомца
public class Breed
{
    private Breed(string title)
    {
        Id = new Guid();
        Title = title;
    }
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    
    public static Result<Breed> Create(string title)
    {
        var breed = new Breed(title);

        return Result.Success(breed);
    } 
}