using System.Diagnostics.CodeAnalysis;
using ScryfallApi.Client.Models;

namespace ScryfallApi.Client;

public class ScryfallResult<TValue>
{
    
    public TValue? Value { get; set; }
    public Error? Error { get; set; }
    
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; set; }
    
    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsError { get; set; }

    private ScryfallResult(TValue? value, Error? error, bool isSuccess)
    {
        Value = value;
        Error = error;
        IsSuccess = isSuccess;
        IsError = !isSuccess;
    }
    
    public static ScryfallResult<TValue> CreateSuccess(TValue value) => new ScryfallResult<TValue>(value, null, true);
    public static ScryfallResult<TValue> CreateError(Error error) => new ScryfallResult<TValue>(default, error, false);
    
    public static implicit operator ScryfallResult<TValue>(TValue value) => CreateSuccess(value);
    public static implicit operator ScryfallResult<TValue>(Error error) => CreateError(error);
    
    public ScryfallResult<TBound> Bind<TBound>(Func<TValue, TBound> success) => IsSuccess ? success(Value) : Error;
    
}