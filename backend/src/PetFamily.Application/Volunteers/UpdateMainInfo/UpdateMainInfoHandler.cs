using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoRequest request,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;

        var fullName = FullName.Create(
            request.Dto.FullName.LastName,
            request.Dto.FullName.FirstName,
            request.Dto.FullName.Patronymic).Value;

        var description = Description.Create(request.Dto.Description).Value;
        var workExperience = WorkExperience.Create(request.Dto.WorkExperience).Value;
        var contactPhone = PhoneNumber.Create(request.Dto.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(fullName, description, workExperience, contactPhone);
        
        var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation(
            "Update main info at volunteer {fullName.LastName} {fullName.FirstName} {fullName.Patronymic} " +
            "with id {volunteerId}",
            fullName.LastName, fullName.FirstName, fullName.Patronymic, request.VolunteerId);
        
        return result;
    }
}