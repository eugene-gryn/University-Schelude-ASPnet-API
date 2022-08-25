namespace BLL.DTO.Models.UserModels.Password;

public interface IPasswordHandler
{
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    /// <summary>
    /// Function to convert passsword 
    /// </summary>
    /// <param name="password">password string for converting</param>
    /// <returns>KeyValuePair, where Key is salt ,Value is password</returns>
    public KeyValuePair<byte[], byte[]> CreatePasswordHash(string password, byte[]? salt = null);
    public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
}