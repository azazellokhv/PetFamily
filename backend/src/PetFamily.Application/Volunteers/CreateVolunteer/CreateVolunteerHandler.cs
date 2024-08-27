using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, Error>> Handle(
        CreateVolunteerRequest request, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        
        var fullNameResult = FullName.Create(request.LastName, request.FirstName, request.Patronymic); 
        if (fullNameResult.IsFailure)
            return fullNameResult.Error;

        var descriptionResult = Description.Create(request.Description);
        if (descriptionResult.IsFailure)
            return descriptionResult.Error;
        
        var workExperienceResult = WorkExperience.Create(request.WorkExperience);
        if (workExperienceResult.IsFailure)
            return workExperienceResult.Error;
        
        var contactPhoneResult = ContactPhone.Create(request.ContactPhone);
        if (contactPhoneResult.IsFailure)
            return contactPhoneResult.Error;
        
        var socialNetworksResult = new List<SocialNetwork>();
        foreach (var socialNetwork in request.SocialNetworks)
        {
            var socialNetworkResult = SocialNetwork.Create(
                socialNetwork.Title, socialNetwork.Link);

            if (socialNetworkResult.IsFailure)
                return socialNetworkResult.Error;

            socialNetworksResult.Add(socialNetworkResult.Value);
        }

        var detailsResult = new List<DetailsForAssistance>();
        foreach (var detail in request.DetailsForAssistance)
        {
            var detailResult = DetailsForAssistance.Create(
                detail.Title, detail.Description, detail.ContactPhoneAssistance, detail.BankCardAssistance);

            if (detailResult.IsFailure)
                return detailResult.Error;

            detailsResult.Add(detailResult.Value);
        }
        
        var volunteer = await _volunteersRepository
            .GetByContactPhone(contactPhoneResult.Value, cancellationToken);

        if (volunteer.IsSuccess)
            return Errors.Volunteer.AlreadyExist();
        
        var volunteerDetailsResult = new VolunteerDetails(socialNetworksResult, detailsResult);
     
        var volunteerResult = Volunteer.Create(
            volunteerId, 
            fullNameResult.Value, 
            descriptionResult.Value,
            workExperienceResult.Value,
            contactPhoneResult.Value,
            volunteerDetailsResult);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error;
        
        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

        return (Guid)volunteerResult.Value.Id;
    }

}