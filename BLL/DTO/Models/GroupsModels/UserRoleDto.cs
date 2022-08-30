using DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BLL.DTO.Models.SubjectModels;
using BLL.DTO.Models.UserModels;

namespace BLL.DTO.Models.GroupsModels;

public class UserRoleDto {
    [Required]
    public int UserId { get; set; }
    public UserDto? User { get; set; }

    [Required]
    public int GroupId { get; set; }
    public GroupDto? Group { get; set; }

    [Required]
    public bool IsModerator { get; set; } = false;

    [Required]
    public bool IsOwner { get; set; } = false;
}