using FluentValidation;
using PetFamily.Application.DTOs;
using PetFamily.Application.Validator;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteersManagement.UpdateMainInfo;

public class UpdateMainInfoCommandValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoCommandValidator()
    {
        RuleFor(r => r.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());
    }
}

public class UpdateMainInfoHandlerDtoValidator : AbstractValidator<UpdateMainInfoDto>
{
    public UpdateMainInfoHandlerDtoValidator()
    {
       
        RuleFor(c => c.FullName)
            .MustBeValueObject(x => FullName.Create(x.LastName, x.FirstName, x.Patronymic));
    
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
    
        RuleFor(c => c.WorkExperience).MustBeValueObject(WorkExperience.Create);

        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}