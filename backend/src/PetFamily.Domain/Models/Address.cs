using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Models;

//адрес нахождения питомца
public class Address 
{
    private Address(string country, string locality, string street, string buildingNumber, string comments)
    {
        Id = new Guid(); 
        Country = country;
        Locality = locality;
        Street = street;
        BuildingNumber = buildingNumber;
        Comments = comments;
    }
    public Guid Id { get; private set; }

    public string Country { get; private set; }
    
    public string Locality { get; private set; }
    
    public string Street { get; private set; }
    
    public string BuildingNumber { get; private set; }
    public string Comments { get; private set; }

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
