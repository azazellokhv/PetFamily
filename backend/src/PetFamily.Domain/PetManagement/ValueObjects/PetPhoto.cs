using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record PetPhoto
{
    public PetPhoto(FilePath fileName, bool isMain)
    {
        FileName = fileName;
        IsMain = isMain;
    }
    public FilePath FileName { get; }
    public bool IsMain { get; }
    

}
