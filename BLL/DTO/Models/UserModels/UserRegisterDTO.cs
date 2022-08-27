using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.DTO.Models.UserModels.Attributes;
using BLL.DTO.Models.UserModels.Password;

namespace BLL.DTO.Models.UserModels;

public class UserRegisterDto {
    private byte[] _hash = { };
    private string _passwordText = string.Empty;
    private byte[] _salt = { };

    [Required(ErrorMessage = "User must have Login")]
    [Column(TypeName = "VARCHAR")]
    [StringLength(20, MinimumLength = 4)]
    [Login]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "User must have name")]
    [StringLength(20, MinimumLength = 4)]
    [Login]
    public string Name { get; set; } = null!;

    [StringLength(15, MinimumLength = 1)]
    [TelegramToken]
    public string? TelegramToken { get; set; }


    [Required]
    [StringLength(24, MinimumLength = 6)]
    [Password]
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