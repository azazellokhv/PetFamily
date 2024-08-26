using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

//адрес нахождения питомца
public record Address 
{
    private Address(
        string country, 
        string locality, 
        string street, 
        string buildingNumber, 
        string comments)
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

    public static Result<Address, Error> Create(
        string country, 
        string locality, 
        string street, 
        string buildingNumber,
        string? comments)
    {
        if (string.IsNullOrWhiteSpace(country) || country.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(country));
        
        if (string.IsNullOrWhiteSpace(locality) || locality.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(locality));
        
        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(street));
        
        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.MAX_NAME_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(street));
   
        return new Address(country, locality, street, buildingNumber, comments);
    }
}
