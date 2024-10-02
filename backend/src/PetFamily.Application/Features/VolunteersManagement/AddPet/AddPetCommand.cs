using PetFamily.Application.DTOs;
using PetFamily.Domain.Shared.Enum;

namespace PetFamily.Application.Features.VolunteersManagement.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Nickname,
    string Description,
    string Color,
    HealthDto Health,
    AddressDto Address,
    string Weight,
    string Height,
    string PhoneNumber,
    bool IsNeutered,
    DateTime Birthday,
    bool IsVaccinated,
    AssistanceStatus AssistanceStatus,
    DetailsForAssistanceDto DetailForAssistance);