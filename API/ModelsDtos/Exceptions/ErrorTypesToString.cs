using BLL.DTO.Models.ExceptionBase;

namespace API.ModelsDtos.Exceptions; 

public static class ErrorTypesToString {
    public static string ErrorEnumToStr(ErrorTypes error) {
        return error switch {
            ErrorTypes.UserNotFound => "user-not-found",
            ErrorTypes.WrongUserId => "wrong-user-id",
            ErrorTypes.WrongRefreshToken => "wrong-refresh-token",
            ErrorTypes.WrongLoginOrPassword => "wrong-credentials-login",
            ErrorTypes.WrongAccessUser => "invalid-operation-access-to-user",
            ErrorTypes.NotUniqueUserLogin => "not-unique-user-login",
            ErrorTypes.UserDoNotHaveImage => "user-image-null",
            ErrorTypes.AdminAction => "restricted-admin-only-action",
            ErrorTypes.PasswordNeeded => "operation-password-needed",
            ErrorTypes.PasswordsNotEquals => "password-compare-not-equal",
            ErrorTypes.UserDoNotHaveThisGroup => "user-do-not-have-this-group",
            ErrorTypes.ErrorTokenValidation => "wrong-token",
            ErrorTypes.WrongId => "wrong-entity-id",
            ErrorTypes.WrongCountNGroups => "wrong-count-user-groups",
            _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
        };
    }
}