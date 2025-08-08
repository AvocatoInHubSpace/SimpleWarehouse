namespace SimpleWarehouse.Application;

public enum ApplicationError
{
    NotFound = 0,
    DuplicateValue,
    ValidationError,
    DeleteNotAllowedResourceInUse,
    UnknownError
}