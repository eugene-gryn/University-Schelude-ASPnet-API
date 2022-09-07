using System.Diagnostics;

namespace BLL.DTO.Models.ExceptionBase;

public enum ErrorTypes {
    WrongLoginOrPassword,
    WrongAccessUser,
    NotUniqueUserLogin,
    UserDoNotHaveImage,
    AdminAction,
    PasswordNeeded,
    PasswordsNotEquals,
    UserNotFound,
    WrongUserId,
    WrongRefreshToken,
    UserDoNotHaveThisGroup,
    ErrorTokenValidation,
    WrongId
}