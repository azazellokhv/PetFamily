using PetFamily.Application.DTOs;

namespace PetFamily.Application.Features.VolunteersManagement.UpdateMainInfo;

public record UpdateMainInfoCommand(
    Guid VolunteerId, 
    UpdateMainInfoDto Dto);