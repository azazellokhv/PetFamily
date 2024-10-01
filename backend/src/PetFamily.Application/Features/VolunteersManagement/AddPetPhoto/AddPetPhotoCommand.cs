using PetFamily.Application.DTOs;

namespace PetFamily.Application.Features.VolunteersManagement.AddPetPhoto;

public record AddPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<UploadFileDto> PetPhotos);