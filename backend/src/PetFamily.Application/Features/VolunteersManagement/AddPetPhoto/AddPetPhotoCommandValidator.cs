using FluentValidation;
using PetFamily.Application.Validator;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteersManagement.AddPetPhoto;

public class AddPetPhotoCommandValidator : AbstractValidator<AddPetPhotoCommand>
{
    public AddPetPhotoCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
        
        RuleFor(d => d.PetId).NotEmpty();
        
        RuleFor(d => d.PetPhotos).NotEmpty().WithError(Errors.General.ValueIsInvalid());
    }
}