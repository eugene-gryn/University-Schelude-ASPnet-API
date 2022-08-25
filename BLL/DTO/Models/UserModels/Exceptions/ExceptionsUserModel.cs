using BLL.DTO.Models.ExceptionBase;

namespace BLL.DTO.Models.UserModels.Exceptions; 

public class WrongLoginCredentialsException : ExceptionModelBase {
    public WrongLoginCredentialsException(string descriptionIssue, string modelName, string actionName) : base(descriptionIssue, modelName, actionName) { }
}
public class NotUniqueLoginUsedException : ExceptionModelBase {
    public NotUniqueLoginUsedException(string descriptionIssue, string modelName, string actionName) : base(descriptionIssue, modelName, actionName) { }
}