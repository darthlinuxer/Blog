namespace Domain.Models;

public class Result<T>
{
    public T Value { get; private set; } 
    public List<string> Errors { get; private set; } = new List<string>();
    public bool IsSuccess { get; private set; }
    public int StatusCode { get; private set; }

    private Result() { }

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value,
            StatusCode = 200 // OK
        };
    }

    public static Result<T> Failure(List<string> errors, int statusCode = 400)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Errors = errors,
            StatusCode = statusCode
        };
    }

    public Result<T> AddError(string error)
    {
        Errors.Add(error);
        return this;
    }

    public Result<T> SetStatusCode(int statusCode)
    {
        StatusCode = statusCode;
        return this;
    }
}