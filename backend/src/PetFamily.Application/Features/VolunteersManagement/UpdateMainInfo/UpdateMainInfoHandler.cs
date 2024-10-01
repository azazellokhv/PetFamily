using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteersManagement.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMainInfoCommand> _validator;
    private readonly ILogger<UpdateMainInfoHandler> _logger;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateMainInfoCommand> validator,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();   
        
        
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var fullName = FullName.Create(
            command.Dto.FullName.LastName,
            command.Dto.FullName.FirstName,
            command.Dto.FullName.Patronymic).Value;

        var description = Description.Create(command.Dto.Description).Value;
        var workExperience = WorkExperience.Create(command.Dto.WorkExperience).Value;
        var contactPhone = PhoneNumber.Create(command.Dto.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(fullName, description, workExperience, contactPhone);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        //var result = await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation(
            "Update main info at volunteer {fullName.LastName} {fullName.FirstName} {fullName.Patronymic} " +
            "with id {volunteerId}",
            fullName.LastName, fullName.FirstName, fullName.Patronymic, command.VolunteerId);
        
        return volunteerResult.Value.Id.Value;
    }
}