using CSharpFunctionalExtensions;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.FileProvider;

public interface IFileProvider
{
    Task<Result<IReadOnlyList<FilePath>, Error>> UploadFiles(
        IEnumerable<FileData> filesData,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> DeleteFiles(
        FileMetaData fileMetaData,
        CancellationToken cancellationToken = default);

    Task<Result<string, Error>> GetUrlFileByName(
        FileMetaData fileMetaData,
        CancellationToken cancellationToken = default);

    Task<UnitResult<Error>> RemoveFile(
        FileInfo fileInfo,
        CancellationToken cancellationToken = default);
}