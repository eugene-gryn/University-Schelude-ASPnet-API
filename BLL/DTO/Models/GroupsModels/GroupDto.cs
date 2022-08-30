using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.DTO.Models.GroupsModels;

public class GroupDto {
    [Key] public int Id { get; set; }

    [Required] [StringLength(50)] public string Name { get; set; } = string.Empty;

    [Required] public bool PrivateType { get; set; }

    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public ICollection<UserRole> UsersRoles { get; set; } = new List<UserRole>();

    public ICollection<Couple> Couples { get; set; } = new List<Couple>();
}