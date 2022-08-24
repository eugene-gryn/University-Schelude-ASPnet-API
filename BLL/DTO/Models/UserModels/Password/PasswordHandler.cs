using System.Security.Cryptography;
using System.Text;

namespace BLL.DTO.Models.UserModels.Password;

public class PasswordHandler : IPasswordHandler {
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
        using var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt) {
        using var hmac = new HMACSHA512(passwordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return passwordHash.SequenceEqual(computedHash);
    }
}