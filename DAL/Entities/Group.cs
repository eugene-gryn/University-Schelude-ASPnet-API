using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Group
{
    [Key] public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required] public User? Creator { get; set; } = new();

    [Required] public List<Subject> Subjects { get; set; } = new();

    [Required] public List<User> Moderators { get; set; } = new();

    [Required] public List<User> Users { get; set; } = new();

    [Required] public List<Couple> Couples { get; set; } = new();

}