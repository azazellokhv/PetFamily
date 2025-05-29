using PetFamily.Application.Abstraction;
using PetFamily.Application.DTOs;
using PetFamily.Domain.Shared.Enum;

namespace PetFamily.Application.VolunteersManagement.Features.AddPet;

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
    DetailsForAssistanceDto DetailForAssistance) : ICommand;