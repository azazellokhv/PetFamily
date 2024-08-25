namespace PetFamily.Application.DTOs;

public record DetailsForAssistanceDTO(
    string Title, 
    string Description, 
    string ContactPhoneAssistance, 
    string BankCardAssistance);