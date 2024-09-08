using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public record UpdateMainInfoRequest(
    Guid VolunteerId, 
    UpdateMainInfoDto Dto);