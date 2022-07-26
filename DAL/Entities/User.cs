using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class User
{
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [StringLength(20)] public string Login { get; set; } = string.Empty;

    [StringLength(20)] public string Name { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }

    public string? ImageLocation { get; set; }

    public byte[] Password { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    [NotMapped] public Settings Settings { get; set; }

    [MaxLength(5)] public List<Group> Groups { get; set; } = new();

    public List<HomeworkTask> Homeworks { get; set; } = new();
}