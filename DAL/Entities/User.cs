using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class User
{
    [Key] public int Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(20)]
    public string Login { get; set; } = string.Empty;

    [Required]
    [MinLength(4)]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    public string? ImageLocation { get; set; }

    [Required] public byte[] Password { get; set; } = null!;

    [Required] public byte[] Salt { get; set; } = null!;

    [Required] public Settings Settings { get; set; } = new();

    public List<Group>? Groups { get; set; }
    public List<Homework>? Homeworks { get; set; }
}