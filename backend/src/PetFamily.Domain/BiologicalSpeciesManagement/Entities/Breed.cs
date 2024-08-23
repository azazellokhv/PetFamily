using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Domain.BiologicalSpeciesManagement.Entities;

//порода питомца
public class Breed : Shared.Entity<BreedId>
{
    //For EF Сore
    private Breed (BreedId id) : base(id)
    {
    }
    private Breed(BreedId breedId, string title) 
        : base(breedId)
    {
        Title = title;
    }
    public string Title { get; private set; }

    public static Result<Breed> Create(BreedId breedId, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<Breed>("Не указана порода питомца");
        
        return new Breed(breedId, title);
    }
}
