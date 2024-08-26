using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record PetPhoto
{
    private PetPhoto(string fileName, bool isMain)
    {
        FileName = fileName;
        IsMain = isMain;
    }

    public string FileName { get;  }
    public bool IsMain { get;  }
    
    public static Result<PetPhoto, Error> Create(string fileName, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Errors.General.ValueIsInvalid(nameof(fileName));
        
        return new PetPhoto(fileName, isMain);
    }

}
