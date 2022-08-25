using System.ComponentModel.DataAnnotations;
using BLL.DTO.Models.UserModels.Password;

namespace BLL.DTO.Models.UserModels;

public class UserRegisterDto {
    private string _passwordText = string.Empty;
    
    private byte[] _hash = { };
    private byte[] _salt = { };

    [Required]
    [MinLength(2)]
    [StringLength(20)]
    public string Login { get; set; } = null!;

    [Required]
    [MinLength(2)]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [MinLength(2)] [StringLength(15)] public string? TelegramToken { get; set; }


    [Required]
    public string Password {
        get => _passwordText;
        set {
            _passwordText = value;
            PasswordSingleton.Password.CreatePasswordHash(_passwordText, out _hash, out _salt);
        }
    }

    /// <summary>
    ///     Convert text password to hash + salt
    /// </summary>
    /// <returns>KeyValuePair, where Key is salt ,Value is password</returns>
    public KeyValuePair<byte[], byte[]> GetPasswordHash() {
        return new KeyValuePair<byte[], byte[]>(_salt, _hash);
    }
}