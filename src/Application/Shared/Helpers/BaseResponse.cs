namespace Application.DTOs.Common;

public class BaseResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static BaseResponse Success(string message = "Operation completed successfully")
    {
        return new BaseResponse
        {
            IsSuccess = true,
            Message = message
        };
    }

    public static BaseResponse Failure(string message, List<string>? errors = null)
    {
        return new BaseResponse
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

public class BaseResponse<T> : BaseResponse
{
    public T? Data { get; set; }

    public static BaseResponse<T> Success(T data, string message = "Operation completed successfully")
    {
        return new BaseResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    public static new BaseResponse<T> Failure(string message, List<string>? errors = null)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>(),
            Data = default
        };
    }
}
