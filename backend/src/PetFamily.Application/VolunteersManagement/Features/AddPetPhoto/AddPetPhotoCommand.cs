using PetFamily.Application.Abstraction;
using PetFamily.Application.DTOs;

namespace PetFamily.Application.VolunteersManagement.Features.AddPetPhoto;

public record AddPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> PetPhotos) : ICommand;