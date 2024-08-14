using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

public class PetPhoto
{
    private PetPhoto(string fileName, bool isMain)
    {
        Id = new Guid();
        FileName = fileName;
        IsMain = isMain;
    }

    public Guid Id { get; private set; }
    public string FileName { get; private set; }
    public bool IsMain { get; private set; }
    
    public static Result<PetPhoto> Create(string fileName, bool isMain)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Result.Failure<PetPhoto>($"Путь к '{nameof(fileName)}' не может быть пустым");
        
        var petPhoto = new PetPhoto(fileName, isMain);
        
        return Result.Success(petPhoto);
    }

}