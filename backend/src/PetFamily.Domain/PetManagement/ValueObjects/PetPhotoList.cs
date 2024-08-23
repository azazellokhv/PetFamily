namespace PetFamily.Domain.PetManagement.ValueObjects;

public record PetPhotoList
{
    public List<PetPhoto> Photos { get; } = default!;
}