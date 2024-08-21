using CSharpFunctionalExtensions;
using PetFamily.Domain.Models;

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
        
        var descriptionResult = request.Description;
        var workExperienceResult = request.WorkExperience;
        var contactPhoneResult = request.ContactPhone;
        var volunteerDetailsResult = new VolunteerDetails();
        
        
        var volunteerResult = new Volunteer(
            volunteerId, 
            fullNameResult.Value, 
            descriptionResult,
            workExperienceResult,
            contactPhoneResult,
            volunteerDetailsResult);
        
        await _volunteersRepository.Add(volunteerResult, cancellationToken);

        return volunteerId.Value;
    }

}