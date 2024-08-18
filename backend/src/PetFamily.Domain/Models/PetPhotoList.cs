namespace PetFamily.Domain.Models;

public record PetPhotoList
{
    public List<PetPhoto> Photos { get; } = default!;
}