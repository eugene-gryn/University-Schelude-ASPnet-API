using Microsoft.EntityFrameworkCore;

namespace BLL.DTO.Models.ExceptionBase;

public class ExceptionModelBase : Exception {
    public string ModelName { get; }
    public string ActionName { get; }

    public ExceptionModelBase(string descriptionIssue, string modelName, string actionName) : base(descriptionIssue) {
        ModelName = modelName;
        ActionName = actionName;
    }
}