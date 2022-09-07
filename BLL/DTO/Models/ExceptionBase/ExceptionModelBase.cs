using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace BLL.DTO.Models.ExceptionBase;

public class ExceptionModelBase : Exception {
    public ErrorTypes ErrorType { get; }
    public int StatusCode;

    public ExceptionModelBase(HttpStatusCode code, ErrorTypes errorType, string errorDescription) : base(errorDescription) {
        ErrorType = errorType;
        StatusCode = (int)code;
    }
}