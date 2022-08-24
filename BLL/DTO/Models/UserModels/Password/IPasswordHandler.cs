namespace BLL.DTO.Models.UserModels.Password;

public interface IPasswordHandler
{
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt);
}