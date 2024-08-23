using CSharpFunctionalExtensions;

namespace PetFamily.Domain.PetManagement.ValueObjects;

//информация о здоровье питомца
public record Health 
{
    private Health(bool isHealthy, string descriptionDisease)
    {
        IsHealthy = isHealthy;
        DescriptionDisease = descriptionDisease;
    }
    
    public bool IsHealthy { get; }
    public string DescriptionDisease { get; }

    public static Result<Health> Create(bool isHealthy, string descriptionDisease)
    {
        if (isHealthy && string.IsNullOrWhiteSpace(descriptionDisease))
            return Result.Failure<Health>("Если питомец болен, необходимо заполнить информацию о заболевании");

        var health = new Health(isHealthy, descriptionDisease);

        return Result.Success(health);
    }

}