using FluentValidation;
using PetFamily.Application.Validator;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(x => FullName.Create(x.LastName, x.FirstName, x.Patronymic));
    
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    
        RuleFor(c => c.WorkExperience).MustBeValueObject(WorkExperience.Create);

        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        
        RuleForEach(c => c.SocialNetworks).ChildRules(s =>
        {
            s.RuleFor(x => new { x.Title, x.Link })
                .MustBeValueObject(y => SocialNetwork.Create(y.Title, y.Link));
        });
        
        RuleForEach(c => c.DetailsForAssistance).ChildRules(s =>
        {
            s.RuleFor(x => new
                {
                    x.Title, x.Description, x.ContactPhoneAssistance, x.BankCardAssistance
                })
                .MustBeValueObject(y => DetailForAssistance.Create(
                    y.Title, y.Description, y.ContactPhoneAssistance, y.BankCardAssistance));
        });
    }
}