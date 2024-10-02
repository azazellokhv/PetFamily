namespace PetFamily.Application.DTOs;

public record UploadFileDto(Stream Content, string FileName, bool IsMain, string ContentType);