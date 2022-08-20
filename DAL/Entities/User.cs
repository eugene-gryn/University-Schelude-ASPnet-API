using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class User {
    [Key] public int Id { get; set; }

    [StringLength(20)] public string Login { get; set; } = string.Empty;

    [StringLength(20)] public string Name { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }

    public string? ImageLocation { get; set; }

    public byte[] Password { get; set; } = { };

    public byte[] Salt { get; set; } = { };

    public Settings Settings { get; set; }

    [Required] public ICollection<UserRole> UsersRoles { get; set; } = new List<UserRole>();


    public ICollection<HomeworkTask> Homework { get; set; } = new List<HomeworkTask>();
}