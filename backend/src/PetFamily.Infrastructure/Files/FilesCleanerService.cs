using Microsoft.Extensions.Logging;
using PetFamily.Application.Files;
using PetFamily.Application.Messaging;
using FileInfo = PetFamily.Application.Files.FileInfo;

namespace PetFamily.Infrastructure.Files;

public class FilesCleanerService : IFilesCleanerService
{
    private readonly IFileProvider _fileProvider;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly ILogger<FilesCleanerService> _logger;

    public FilesCleanerService(
        IFileProvider fileProvider,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        ILogger<FilesCleanerService> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
        _messageQueue = messageQueue;
    }
    public async Task Process(CancellationToken cancellationToken)
    {
        var filesInfo = await _messageQueue.ReadAsync(cancellationToken);

        foreach (var fileInfo in filesInfo)
        {
            await _fileProvider.RemoveFile(fileInfo, cancellationToken);
        }
    }
}