namespace Taskorium.ServiceDefaults.Result;

public class Result<T>
{
    protected Result(bool isSuccess, T value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    private bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }
    public T Value { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default!, error);
    }
}
