using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Domain.BiologicalSpeciesManagement.ValueObjects;

public record PetType
{
    private PetType(BiologicalSpeciesId biologicalSpeciesId, BreedId breedId)
    {
        BiologicalSpeciesId = biologicalSpeciesId;
        BreedId = breedId;
    }
    
    public BiologicalSpeciesId BiologicalSpeciesId { get; }
    public BreedId BreedId { get; }

    public static Result<PetType> Create(BiologicalSpeciesId biologicalSpeciesId, BreedId breedId)
    {
        var petType = new PetType(biologicalSpeciesId, breedId);
        
        return Result.Success(petType);
    }

}