namespace PetFamily.Domain.Models;

public class DetailsForAssistance 
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }
    
    public string Description { get; private set; }
    
    public int ContactPhoneAssistance { get; private set; }
    
    public int BankCardAssistance { get; private set; }
    
}