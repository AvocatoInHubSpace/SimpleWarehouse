using System.Text.Json.Serialization;

namespace SimpleWarehouse.Application;


public class Result<TError> where TError : Enum
{
    public bool IsSuccess { get; }
    
    public string? ErrorMessage { get; }
    
    public TError? Error { get; }

    [JsonConstructor]
    public Result(bool isSuccess, TError? error = default, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<TError> Success() => new (true);
    public static Result<TError> Failure(TError applicationError, string? errorMessage = null) => new (false, applicationError, errorMessage);
}

public class Result<T, TError> : Result<TError> where TError : Enum
{
    public T? Value { get; }

    [JsonConstructor]
    public Result(bool isSuccess, T? value, TError? error = default, string? errorMessage = null)
        : base(isSuccess, error, errorMessage)
    {
        Value = value;
    }

    public static Result<T, TError> Success(T value) =>
        new (true, value);

    public new static Result<T, TError> Failure(TError error, string? errorMessage = null) =>
        new (false, default, error, errorMessage);

}