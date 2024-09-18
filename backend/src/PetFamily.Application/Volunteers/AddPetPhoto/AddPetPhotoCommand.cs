using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.AddPetPhoto;

public record AddPetPhotoCommand(
    Guid VolunteerId,
    Guid PetId,
    IEnumerable<PetPhotoDto> PetPhotos);