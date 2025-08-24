// Core/Error.cs
public enum ErrorKind
{
    Validation,
    Conflict,
    Forbidden,   // <-- lägg till denna
    NotFound,
    Unexpected
}

public sealed record Error(
    string Code,
    string Message,
    ErrorKind Kind = ErrorKind.Validation,
    string? Field = null
);

public static class Err
{
    public static Error Validation(string field, string message, string code = "validation") =>
        new(code, message, ErrorKind.Validation, field);

    public static Error Conflict(string code, string message) =>
        new(code, message, ErrorKind.Conflict);

    public static Error Forbidden(string code, string message) =>   // <-- lägg till denna
        new(code, message, ErrorKind.Forbidden);

    public static Error NotFound(string code, string message) =>
        new(code, message, ErrorKind.NotFound);

    public static Error Unexpected(string message, string code = "unexpected") =>
        new(code, message, ErrorKind.Unexpected);
}