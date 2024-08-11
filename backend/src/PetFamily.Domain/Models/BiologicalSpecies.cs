using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//вид питомца
public class BiologicalSpecies 
{
    private BiologicalSpecies(string title)
    {
        Id = new Guid();
        Title = title;
    }
    public Guid Id { get; private set; }

    public string Title { get; private set; }
    
    public static Result<BiologicalSpecies> Create(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<BiologicalSpecies>("Не указан вид питомца");

        var biologicalSpecies = new BiologicalSpecies(title);

        return Result.Success(biologicalSpecies);
    }
}