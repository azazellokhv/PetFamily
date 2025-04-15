using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public class Position : ValueObject
{
    public static Position First = new(1);
    private Position(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public Result<Position, Error> Forward() => Create(Value + 1);
    
    public Result<Position, Error> Back() => Create(Value - 1);
    public static Result<Position, Error> Create(int value)
    {
        if (value < 1)
            return Errors.General.ValueIsInvalid("position must be greater than 0");

        return new Position(value);
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public static implicit operator int(Position position) => position.Value;
   
}