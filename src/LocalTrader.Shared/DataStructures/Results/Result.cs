using System.Diagnostics.CodeAnalysis;

namespace LocalTrader.Shared.DataStructures.Results;



public sealed record Result<T>
{
    private Result(Error? error, T? value)
    {
        Error = error;
        IsSuccess = error is null;
        Value = value;
    }
    
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public new bool IsFailure  => !IsSuccess;

    public Error? Error { get; }
    public T? Value { get; }
    
    public static Result<T> CreateSuccess(T value) => new(null, value);
    public static Result<T> CreateError(Error error) => new(error, default);
    
    public static implicit operator Result<T>(T value) => CreateSuccess(value);
    public static implicit operator Result<T>(Error error) => CreateError(error);
}

