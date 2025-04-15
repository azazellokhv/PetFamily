using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.FileProvider;

public record FileData(Stream Stream, FileInfo Info);
//public record FileData(Stream Stream, string ObjectName);

