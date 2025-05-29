using PetFamily.Application.DTOs;
using PetFamily.Application.VolunteersManagement.Features.AddPet;
using PetFamily.Domain.Shared.Enum;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record AddPetRequest(
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
    DetailsForAssistanceDto DetailForAssistance)
{
    public AddPetCommand ToCommand(Guid volunteerId) => new(
        volunteerId,
        Nickname,
        Description,
        Color,
        Health,
        Address,
        Weight,
        Height,
        PhoneNumber,
        IsNeutered,
        Birthday,
        IsVaccinated,
        AssistanceStatus,
        DetailForAssistance);
}