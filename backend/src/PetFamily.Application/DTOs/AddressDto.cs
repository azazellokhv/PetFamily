namespace PetFamily.Application.DTOs;

public record AddressDto(
    string Country, 
    string Locality, 
    string Street, 
    string BuildingNumber, 
    string Comments);