
namespace SimpleWarehouse.Application.Resources;

public static class ResourceErrorResult
{
    public static Result<T, ApplicationError> Failure<T>(ApplicationError error)
    {
        var errorMessage = ErrorMessage(error);
        return Result<T, ApplicationError>.Failure(error, errorMessage);
    }
    
    public static Result<ApplicationError> Failure(ApplicationError error)
    {
        var errorMessage = ErrorMessage(error);
        return Result<ApplicationError>.Failure(error, errorMessage);
    }

    private static string ErrorMessage(ApplicationError error)
    {
        return error switch
        {
            ApplicationError.NotFound => "Ресурс с указанным Id не найден",
            ApplicationError.DuplicateValue => "В системе уже зарегистрирован ресурс с таким наименованием",
            ApplicationError.ValidationError => "Введены некорректные данные",
            ApplicationError.DeleteNotAllowedResourceInUse => "Невозможно удалить ресурс, так как он используется",
            _ => "Внутренняя ошибка, попробуйте позже"
        };
    }
}