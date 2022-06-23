using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class User
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(20)] public string Login { get; set; } = string.Empty;

    [Required] [StringLength(20)] public string Name { get; set; } = string.Empty;

    [Required] public bool IsAdmin { get; set; }

    public string? ImageLocation { get; set; }

    [Required] public byte[] Password { get; set; } = null!;

    [Required] public byte[] Salt { get; set; } = null!;

    [Required] public Settings Settings { get; set; } = new();

    [Required] [MaxLength(5)] public List<Group> Groups { get; set; } = new();

    [Required] public List<HomeworkTask> Homeworks { get; set; } = new();
}