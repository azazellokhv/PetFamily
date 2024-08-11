using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//информация о здоровье питомца
public class Health 
{
    private Health(bool isHealthy, string descriptionDisease)
    {
        Id = new Guid();
        IsHealthy = isHealthy;
        DescriptionDisease = descriptionDisease;
    }
    
    public Guid Id { get; private set; }
    public bool IsHealthy { get; private set; }
    public string DescriptionDisease { get; private set; }

    public static Result<Health> Create(bool isHealthy, string descriptionDisease)
    {
        if (isHealthy && string.IsNullOrWhiteSpace(descriptionDisease))
            return Result.Failure<Health>("Если питомец болен, необходимо заполнить информацию о заболевании");

        var health = new Health(isHealthy, descriptionDisease);

        return Result.Success(health);
    }

}