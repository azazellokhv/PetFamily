using CSharpFunctionalExtensions;

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
    
    public static Result<PetPhoto> Create(string fileName, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure<PetPhoto>($"Путь к '{nameof(fileName)}' не может быть пустым");
        
        return new PetPhoto(fileName, isMain);
    }

}
