using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record PetPhoto
{
    [JsonConstructor]
    public PetPhoto(FilePath filePath, bool isMain)
    {
        FilePath = filePath;
        IsMain = isMain;
    }
    public FilePath FilePath { get; }
    public bool IsMain { get; }
    
    public static Result<PetPhoto, Error> Create(FilePath filePath, bool isMain)
    {
        return new PetPhoto(filePath, isMain);
    }
    

}
