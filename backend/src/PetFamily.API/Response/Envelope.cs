﻿namespace PetFamily.API.Response;

public record ResponseError(string ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    private Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        TimeGanerated = DateTime.Now.ToUniversalTime();
    }

    public object? Result { get; }
    public List<ResponseError> Errors { get; }
    public DateTime TimeGanerated { get; }

    public static Envelope Ok(object? result = null) =>
        new Envelope(result, []);
    
    public static Envelope Error(IEnumerable<ResponseError> errors) =>
        new Envelope(null, errors);
}