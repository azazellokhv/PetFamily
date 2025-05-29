using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstraction;
using PetFamily.Application.Database;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.VolunteersManagement.Features.Create;

public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command, 
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
                
        var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        var volunteerId = VolunteerId.NewVolunteerId();
        
        try
        {
            var fullName = FullName.Create(
                command.FullName.LastName,
                command.FullName.FirstName,
                command.FullName.Patronymic).Value;

            var description = Description.Create(command.Description).Value;

            var workExperience = WorkExperience.Create(command.WorkExperience).Value;

            var contactPhone = PhoneNumber.Create(command.PhoneNumber).Value;

            var socialNetworks = command.SocialNetworks
                .Select(s => SocialNetwork.Create(s.Title, s.Link).Value);
            var socialNetworksList = new SocialNetworksList(socialNetworks);

            var detailsForAssistance = command.DetailsForAssistance
                .Select(d => DetailForAssistance.Create(
                    d.Title, d.Description, d.ContactPhoneAssistance, d.BankCardAssistance).Value);
            var volunteerDetailsList = new VolunteerDetailsList(detailsForAssistance);

            var volunteer = await _volunteersRepository
                .GetByContactPhone(contactPhone, cancellationToken);

            if (volunteer.IsSuccess)
                return Errors.General.AlreadyExist().ToErrorList();

            var volunteerResult = Volunteer.Create(
                volunteerId,
                fullName,
                description,
                workExperience,
                contactPhone,
                socialNetworksList,
                volunteerDetailsList);
            if (volunteerResult.IsFailure)
                return volunteerResult.Error.ToErrorList();

            await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);
            
            await _unitOfWork.SaveChanges(cancellationToken);

            _logger.LogInformation(
                "Create volunteer with id {volunteerId}", volunteerId);

            transaction.Commit();
            
            return (Guid)volunteerResult.Value.Id;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,
                "Can not created volunteer with id {volunteerId}", volunteerId);
            
            transaction.Rollback();

            return Error.Failure("Can not add volunteer {volunteerId}", "volunteer.failure").ToErrorList();
        }
        
    }
}