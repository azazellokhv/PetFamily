namespace PetFamily.Domain.PetManagement.ValueObjects;

public record PetPhotoList
{
    private PetPhotoList() { }

    public PetPhotoList(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }
    public IReadOnlyList<PetPhoto> PetPhotos { get; }
}