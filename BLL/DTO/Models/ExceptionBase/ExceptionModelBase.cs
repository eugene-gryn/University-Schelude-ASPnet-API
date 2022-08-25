using Microsoft.EntityFrameworkCore;

namespace BLL.DTO.Models.ExceptionBase;

public class ExceptionModelBase : Exception {
    public int StatusCode { get; }
    public string ModelName { get; }
    public string ActionName { get; }

    public ExceptionModelBase(int statusCode, string descriptionIssue, string modelName, string actionName) : base(descriptionIssue) {
        StatusCode = statusCode;
        ModelName = modelName;
        ActionName = actionName;
    }
}