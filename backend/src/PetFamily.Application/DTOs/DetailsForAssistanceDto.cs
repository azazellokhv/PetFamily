namespace PetFamily.Application.DTOs;

public record DetailsForAssistanceDto(
    string Title, 
    string Description, 
    string ContactPhoneAssistance, 
    string BankCardAssistance);