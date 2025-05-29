using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.Files;

public record FileInfo(FilePath FilePath, string BucketName);