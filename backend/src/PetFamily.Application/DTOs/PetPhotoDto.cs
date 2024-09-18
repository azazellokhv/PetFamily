namespace PetFamily.Application.DTOs;

public record PetPhotoDto(Stream Content, string FileName, bool IsMain, string ContentType);