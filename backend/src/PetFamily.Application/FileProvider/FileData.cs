using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.FileProvider;

public record FileData(Stream Stream, FilePath FilePath, string BucketName);
//public record FileData(Stream Stream, string ObjectName);

