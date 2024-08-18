using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//вид питомца
public class BiologicalSpecies : Shared.Entity<BiologicalSpeciesId>
{
    private readonly List<Breed> _breeds = [];
    //For EF Сore
    private BiologicalSpecies (BiologicalSpeciesId id) : base(id)
    {
    }
    private BiologicalSpecies(BiologicalSpeciesId biologicalSpeciesId, string name)
        : base(biologicalSpeciesId)
    {
        Name = name; 
    }
    
    public string Name { get; private set; }
    public IReadOnlyList<Breed> Breeds => _breeds;
    public void AddBreed(Breed breed)
    {
        _breeds.Add(breed);
    }
    public static Result<BiologicalSpecies> Create(
        BiologicalSpeciesId biologicalSpeciesId, 
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<BiologicalSpecies>("Не указан вид питомца");

        return new BiologicalSpecies(biologicalSpeciesId, name);
    }
}