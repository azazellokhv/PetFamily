using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        ILogger<CreateVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        
        var fullName = FullName.Create(
            request.FullName.LastName, 
            request.FullName.FirstName, 
            request.FullName.Patronymic).Value; 

        var description = Description.Create(request.Description).Value;
        
        var workExperience = WorkExperience.Create(request.WorkExperience).Value;
        
        var contactPhone = PhoneNumber.Create(request.PhoneNumber).Value;
        
        var socialNetworks = request.SocialNetworks
            .Select(s => SocialNetwork.Create(s.Title, s.Link).Value);
        var socialNetworksList = new SocialNetworksList(socialNetworks);

        var detailsForAssistance = request.DetailsForAssistance
            .Select(d => DetailForAssistance.Create(
            d.Title, d.Description, d.ContactPhoneAssistance, d.BankCardAssistance).Value);
        var volunteerDetailsList = new VolunteerDetailsList(detailsForAssistance);

        var volunteer = await _volunteersRepository
            .GetByContactPhone(contactPhone, cancellationToken);

        if (volunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist();
     
        var volunteerResult = Volunteer.Create(
            volunteerId, 
            fullName, 
            description,
            workExperience,
            contactPhone,
            socialNetworksList,
            volunteerDetailsList);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation(
            "Create volunteer with id {volunteerId}", volunteerId);

        return (Guid)volunteerResult.Value.Id;
    }
}