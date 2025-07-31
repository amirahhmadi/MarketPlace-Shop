namespace GameOnline.Common.Core;

public enum OperationCode
{
    Success = 200,
    Error = 500,
    NotFound = 404,
    Duplicate = 409,
    ValidationError = 422,
    Unauthorized = 401,
    Forbidden = 403
}