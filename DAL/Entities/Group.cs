using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Group
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required] public User Creator { get; set; } = new();

    [Required] public List<Subject> Subjects { get; set; } = new();

    public List<User>? Moderators { get; set; }

    public List<User>? Users { get; set; }

    public List<Couple>? Couples { get; set; }

}