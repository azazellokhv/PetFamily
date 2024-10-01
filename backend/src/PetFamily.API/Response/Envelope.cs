using PetFamily.Domain.Shared;

namespace PetFamily.API.Response;

public record ResponseError(string ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    private Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        TimeGanerated = DateTime.Now.ToUniversalTime();
    }

    public object? Result { get; }
    public ErrorList? Errors { get; }
    public DateTime TimeGanerated { get; }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Error(ErrorList errors) =>
        new(null, errors);
}