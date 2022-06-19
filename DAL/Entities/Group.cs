using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Group
{
    [Key] public int Id { get; set; }

    [Required]
    [MinLength(4)]
    [MaxLength(50)]
    public string Name { get; set; }

    public User? Creator { get; set; }
    public List<User>? Users { get; set; }
    public List<Couple>? Couples { get; set; }
}