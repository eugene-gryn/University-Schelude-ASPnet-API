using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class HomeworkTask {
    [Key]
    public int Id { get; set; }

    [MaxLength(1000)] public string? Description { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Deadline { get; set; }

    [Required] public byte Priority { get; set; } = 5;

    [Required] public int UserId { get; set; }
    public User? User { get; set; }

    [Required] public int SubjectId { get; set; }
    public Subject? Subject { get; set; }
}