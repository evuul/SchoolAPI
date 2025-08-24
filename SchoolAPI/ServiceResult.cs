// Core/ServiceResult.cs
public class ServiceResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public IReadOnlyList<Error> Errors { get; }

    private ServiceResult(bool ok, T? value, IReadOnlyList<Error> errors)
        => (IsSuccess, Value, Errors) = (ok, value, errors);

    public static ServiceResult<T> Ok(T value) => new(true, value, Array.Empty<Error>());
    public static ServiceResult<T> Fail(params Error[] errors) => new(false, default, errors);
}

public static class ServiceResult
{
    public static ServiceResult<Unit> Ok() => ServiceResult<Unit>.Ok(Unit.Value);
    public static ServiceResult<Unit> Fail(params Error[] errors) => ServiceResult<Unit>.Fail(errors);
}

public readonly struct Unit { public static readonly Unit Value = new(); }