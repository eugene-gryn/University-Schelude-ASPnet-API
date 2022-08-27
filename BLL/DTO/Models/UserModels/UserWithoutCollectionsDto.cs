using DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Models.UserModels;

public class UserWithoutCollectionsDto {
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "User must have Login")]
    [Column(TypeName = "VARCHAR")]
    [StringLength(20, MinimumLength = 4)]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "User must have name")]
    [StringLength(20, MinimumLength = 4)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "VARCHAR")]
    [StringLength(15, MinimumLength = 1)]
    public string? TelegramToken { get; set; }

    public bool IsAdmin { get; set; } = false;

    public string? ImageLocation { get; set; }
}