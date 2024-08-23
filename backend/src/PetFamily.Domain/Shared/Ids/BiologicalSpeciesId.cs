namespace PetFamily.Domain.Shared.Ids;

public record BiologicalSpeciesId
{
    private BiologicalSpeciesId(Guid value)
    {
        Value = value;
    }
    public Guid Value { get; }

    public static BiologicalSpeciesId NewBiologicalSpeciesId() => new(Guid.NewGuid());
    public static BiologicalSpeciesId Empty() => new(Guid.Empty);
    public static BiologicalSpeciesId Create(Guid id) => new(id);

}