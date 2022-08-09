using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class Group
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key] public int Id { get; set; }

    [Required] [StringLength(50)] public string Name { get; set; } = string.Empty;

    [Required] public bool PrivateType { get; set; }

    [Required] public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    [Required] public ICollection<UserRole> UsersRoles { get; set; } = new List<UserRole>();

    [Required] public ICollection<Couple> Couples { get; set; } = new List<Couple>();
}