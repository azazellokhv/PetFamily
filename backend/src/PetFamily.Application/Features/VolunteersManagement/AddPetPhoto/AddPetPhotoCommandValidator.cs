using FluentValidation;

namespace PetFamily.Application.Features.VolunteersManagement.AddPetPhoto;

public class AddPetPhotoCommandValidator : AbstractValidator<AddPetPhotoCommand>
{
    public AddPetPhotoCommandValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty();
        RuleFor(d => d.PetId).NotEmpty();
    }
}