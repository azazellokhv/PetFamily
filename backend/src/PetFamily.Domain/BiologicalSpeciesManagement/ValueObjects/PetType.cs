using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Domain.BiologicalSpeciesManagement.ValueObjects;

public record PetType
{
    private PetType(BiologicalSpeciesId biologicalSpeciesId, Guid breedId)
    {
        BiologicalSpeciesId = biologicalSpeciesId;
        BreedId = breedId;
    }
    
    public BiologicalSpeciesId BiologicalSpeciesId { get; }
    public Guid BreedId { get; }

    public static Result<PetType> Create(BiologicalSpeciesId biologicalSpeciesId, Guid breedId)
    {
        var petType = new PetType(biologicalSpeciesId, breedId);
        
        return Result.Success(petType);
    }

}