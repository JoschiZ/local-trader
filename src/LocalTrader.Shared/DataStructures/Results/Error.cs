namespace LocalTrader.Shared.DataStructures.Results;

public record Error(string Message, Error? InnerError = null);
public record NotFoundError(string Message, Error? InnerError = null) : Error(Message, InnerError);


public static class ErrorExtensions
{
    extension(Error)
    {
        public static NotFoundError NotFound => new("Not found");
    }
}