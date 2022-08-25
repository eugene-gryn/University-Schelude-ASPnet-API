namespace BLL.DTO.Models.UserModels.Password;

public static class PasswordSingleton {
    private static IPasswordHandler? _password;

    public static IPasswordHandler Password => _password ??= new PasswordHandler();
}