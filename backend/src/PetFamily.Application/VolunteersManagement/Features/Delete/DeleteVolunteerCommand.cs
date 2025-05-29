using PetFamily.Application.Abstraction;

namespace PetFamily.Application.VolunteersManagement.Features.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;