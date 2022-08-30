using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.DTO.Models.GroupsModels;
using BLL.DTO.Models.HomeworkTaskModels;
using BLL.DTO.Models.UserModels.Attributes;
using DAL.Entities;

namespace BLL.DTO.Models.UserModels;

public class UserDto {
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "User must have Login")]
    [Column(TypeName = "VARCHAR")]
    [StringLength(20, MinimumLength = 4)]
    [Login]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "User must have name")]
    [StringLength(20, MinimumLength = 4)]
    [Login]
    public string Name { get; set; } = null!;

    [Column(TypeName = "VARCHAR")]
    [StringLength(15, MinimumLength = 1)]
    [TelegramToken]
    public string? TelegramToken { get; set; }

    public bool IsAdmin { get; set; } = false;

    public Settings Settings { get; set; } = new();

    [MaxLength(5)]
    public ICollection<UserRoleDto> UsersRoles { get; set; } = new List<UserRoleDto>();


    public ICollection<HomeworkTaskDto> Homework { get; set; } = new List<HomeworkTaskDto>();
}