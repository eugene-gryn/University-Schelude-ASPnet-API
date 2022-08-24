using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Models.UserModels;

public class UserLoginDto {
    [StringLength(20)] public string Login { get; set; } = null!;

    [StringLength(14)] public string Password { get; set; } = null!;
}