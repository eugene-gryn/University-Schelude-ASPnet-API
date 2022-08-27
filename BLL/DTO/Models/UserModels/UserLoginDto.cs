using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.DTO.Models.UserModels.Attributes;

namespace BLL.DTO.Models.UserModels;

public class UserLoginDto {
    [Required(ErrorMessage = "User must have Login")]
    [Column(TypeName = "VARCHAR")]
    [StringLength(20, MinimumLength = 4)]
    [Login]
    public string Login { get; set; } = null!;

    [Required]
    [StringLength(24, MinimumLength = 6)]
    public string Password { get; set; } = null!;
}