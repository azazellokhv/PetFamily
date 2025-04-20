using FluentValidation;
using PetFamily.Application.DTOs;
using PetFamily.Application.Validator;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Features.VolunteersManagement.AddPetPhoto;

public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
{
    public UploadFileDtoValidator()
    {
        RuleFor(u => u.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(u => u.Content).Must(c => c.Length < 10000000);
    }
}