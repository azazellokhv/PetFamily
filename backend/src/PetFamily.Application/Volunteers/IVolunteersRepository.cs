using CSharpFunctionalExtensions;
using PetFamily.Domain.Models;

namespace PetFamily.Application.Volunteers;

public interface IVolunteersRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);
    Task<Result<Volunteer>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default);
}