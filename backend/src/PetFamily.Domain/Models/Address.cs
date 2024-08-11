namespace PetFamily.Domain.Models;

//адрес нахождения питомца
public class Address 
{
    public Guid Id { get; private set; }

    public string Country { get; private set; }
    
    public string Locality { get; private set; }
    
    public string Street { get; private set; }
    
    public string BuildingNumber { get; private set; }
    
    public string Comments { get; private set; }
}