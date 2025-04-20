using FluentValidation;
using PetFamily.Application.Validator;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteersManagement.AddPetPhoto;

public class AddPetPhotoCommandValidator : AbstractValidator<AddPetPhotoCommand>
{
    public AddPetPhotoCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(d => d.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleForEach(d => d.PetPhotos).SetValidator(new UploadFileDtoValidator());
    }
}