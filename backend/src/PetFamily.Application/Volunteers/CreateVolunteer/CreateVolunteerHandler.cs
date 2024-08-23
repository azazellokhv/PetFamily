using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    
    public async Task<Result<Guid, string>> Handle(
        CreateVolunteerRequest request, CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        
        var fullNameResult = FullName.Create(request.LastName, request.FirstName, request.Patronymic); 
        if (fullNameResult.IsFailure)
            return fullNameResult.Error;

        var descriptionResult = Description.Create(request.Description);
        var workExperienceResult = WorkExperience.Create(request.WorkExperience);
        var contactPhoneResult = ContactPhone.Create(request.ContactPhone);
        var volunteerDetailsResult = new VolunteerDetails();

        var volunteerResult = Volunteer.Create(
            volunteerId, 
            fullNameResult.Value, 
            descriptionResult.Value,
            workExperienceResult.Value,
            contactPhoneResult.Value,
            volunteerDetailsResult);
        
        await _volunteersRepository.Add(volunteerResult.Value, cancellationToken);

        return volunteerId.Value;
    }

}