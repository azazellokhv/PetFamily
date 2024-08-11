namespace PetFamily.Domain.Models;

public class Health 
{
    public Guid Id { get; private set; }

    public bool IsHealthy { get; private set; }
    
    public string DescriptionDisease { get; private set; }
    
}