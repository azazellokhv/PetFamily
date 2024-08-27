using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record WorkExperience
{
    private WorkExperience(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<WorkExperience, Error> Create(int value)
    {
        if (value < Constants.MIN_WORK_EXPERIENCE || value > Constants.MAX_WORK_EXPERIENCE)
            return Errors.General.ValueIsInvalid(nameof(WorkExperience));
   
        return new WorkExperience(value);   
        
    }
}