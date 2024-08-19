using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//адрес нахождения питомца
public record Address 
{
    private Address(string country, string locality, string street, string buildingNumber, string comments)
    {
        Country = country;
        Locality = locality;
        Street = street;
        BuildingNumber = buildingNumber;
        Comments = comments;
    }
    public string Country { get; }
    public string Locality { get; }
    public string Street { get; }
    public string BuildingNumber { get; }
    public string Comments { get; }

    public static Result<Address> Create(string country, string locality, string street, string buildingNumber,
        string comments)
    {
        if (string.IsNullOrWhiteSpace(country) || 
            string.IsNullOrWhiteSpace(locality) ||
            string.IsNullOrWhiteSpace(street) ||
            string.IsNullOrWhiteSpace(buildingNumber))
            return Result.Failure<Address>("Не полностью указан адрес нахождения питомца");
        
        var address = new Address(country, locality, street, buildingNumber, comments);

        return Result.Success(address);
    }
}
