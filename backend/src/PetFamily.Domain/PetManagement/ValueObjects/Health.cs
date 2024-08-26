using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

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

    public static Result<Health, Error> Create(bool isHealthy, string descriptionDisease)
    {
        if (isHealthy && string.IsNullOrWhiteSpace(descriptionDisease))
            return Errors.General.ValueIsInvalid(nameof(Health));

        return new Health(isHealthy, descriptionDisease);
    }

}