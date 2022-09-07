using BLL.DTO.Models.ExceptionBase;

namespace API.ModelsDtos.Exceptions;

public class Error {
    public Error(ExceptionModelBase baseError) {
        ErrorType = ErrorTypesToString.ErrorEnumToStr(baseError.ErrorType);
        ErrorMessage = baseError.Message;
    }
    public Error(string errorType, string errorMessage) {
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    public string ErrorType { get; }
    public string ErrorMessage { get; }
}