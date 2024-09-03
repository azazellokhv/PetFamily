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
        
        var socialNetworks = new List<SocialNetwork>();
        foreach (var socialNetwork in request.SocialNetworks)
        {
            var socialNetworkResult = SocialNetwork.Create(
                socialNetwork.Title, socialNetwork.Link).Value;

            socialNetworks.Add(socialNetworkResult);
        }

        var details = new List<DetailsForAssistance>();
        foreach (var detail in request.DetailsForAssistance)
        {
            var detailResult = DetailsForAssistance.Create(
                detail.Title, detail.Description, detail.ContactPhoneAssistance, detail.BankCardAssistance).Value;

            details.Add(detailResult);
        }
        
        var volunteer = await _volunteersRepository
            .GetByContactPhone(contactPhone, cancellationToken);

        if (volunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist();
        
        var volunteerDetailsResult = new VolunteerDetails(socialNetworks, details);
     
        var volunteerResult = Volunteer.Create(
            volunteerId, 
            fullName, 
            description,
            workExperience,
            contactPhone,
            volunteerDetailsResult);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation(
            "Create volunteer {fullName.LastName} {fullName.FirstName} {fullName.Patronymic} with id {volunteerId}",
            fullName.LastName, fullName.FirstName, fullName.Patronymic, volunteerId);

        return (Guid)volunteerResult.Value.Id;
    }

}