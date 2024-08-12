using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//вид питомца
public class BiologicalSpecies 
{
    private BiologicalSpecies(string name, Breed breed)
    {
        Id = new Guid();
        Name = name;
        Breed = breed;
    }
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Breed Breed { get; private set; }
    
    public static Result<BiologicalSpecies> Create(string name, Breed breed)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<BiologicalSpecies>("Не указан вид питомца");

        var biologicalSpecies = new BiologicalSpecies(name, breed);

        return Result.Success(biologicalSpecies);
    }
}