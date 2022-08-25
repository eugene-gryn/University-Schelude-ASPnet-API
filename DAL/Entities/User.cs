using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class User {
    [Key] public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [StringLength(20)]
    public string Login { get; set; } = null!;

    [Required]
    [MinLength(2)]
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [MinLength(2)]
    [StringLength(15)]
    public string? TelegramToken { get; set; }

    public bool IsAdmin { get; set; } = false;

    public string? ImageLocation { get; set; }

    public byte[] Password { get; set; } = { };

    public byte[] Salt { get; set; } = { };

    public Settings Settings { get; set; } = new();
    public Tokens Token { get; set; } = new();

    public ICollection<UserRole> UsersRoles { get; set; } = new List<UserRole>();


    public ICollection<HomeworkTask> Homework { get; set; } = new List<HomeworkTask>();
}