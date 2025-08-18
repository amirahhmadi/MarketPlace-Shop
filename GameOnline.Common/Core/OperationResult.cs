using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameOnline.Common.Core;

public class OperationResult<T>
{
    public bool IsSuccess { get; set; }
    public OperationCode Code { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }

    public static OperationResult<T> Success(T data, string message = OperationResultMessage.Success)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Code = OperationCode.Success,
            Message = message,
            Data = data
        };
    }

    public static OperationResult<T> Error(string message = OperationResultMessage.Error)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Code = OperationCode.Error,
            Message = message,
            Data = default
        };
    }

    public static OperationResult<T> NotFound(string message = OperationResultMessage.NotFound)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Code = OperationCode.NotFound,
            Message = message,
            Data = default
        };
    }

    public static OperationResult<T> Duplicate(string message = OperationResultMessage.Duplicate)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Code = OperationCode.Duplicate,
            Message = message,
            Data = default
        };
    }

    public static OperationResult<T> Unauthorized(string message = OperationResultMessage.Unauthorized)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Code = OperationCode.Unauthorized,
            Message = message,
            Data = default
        };
    }

    public static OperationResult<int> Success(string message = OperationResultMessage.Success)
    {
        return new OperationResult<int>
        {
            IsSuccess = true,
            Code = OperationCode.Success,
            Message = message,
            Data = default
        };
    }
}