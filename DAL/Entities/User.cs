using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class User {
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

    public byte[] Password { get; set; } = { };

    public byte[] Salt { get; set; } = { };

    public Settings Settings { get; set; } = new();
    public Tokens Token { get; set; } = new();

    public ICollection<UserRole> UsersRoles { get; set; } = new List<UserRole>();


    public ICollection<HomeworkTask> Homework { get; set; } = new List<HomeworkTask>();
}