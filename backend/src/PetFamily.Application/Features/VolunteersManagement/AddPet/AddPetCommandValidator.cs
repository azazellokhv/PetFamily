using FluentValidation;
using PetFamily.Application.Validator;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.Features.VolunteersManagement.AddPet;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
            RuleFor(c => c.Nickname).MustBeValueObject(Nickname.Create);
            
            RuleFor(c => c.Description).MustBeValueObject(Description.Create);
            
            RuleFor(c => c.Color).MustBeValueObject(Color.Create);
            
            RuleFor(c => c.Address)
                .MustBeValueObject(x => Address.Create(
                    x.Country, x.Locality, x.Street, x.BuildingNumber, x.Comments));
        
            RuleFor(c => c.Weight).MustBeValueObject(Weight.Create);
            
            RuleFor(c => c.Height).MustBeValueObject(Height.Create);
            
            RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}