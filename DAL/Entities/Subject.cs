using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class Subject {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    [Required] [StringLength(50)] public string Name { get; set; } = string.Empty;

    [Column(TypeName = "VARCHAR")]
    [DataType(DataType.Url)]
    [StringLength(200)]
    public string? Url { get; set; }

    [StringLength(50)] public string? Location { get; set; }

    [StringLength(50)] public string? Teacher { get; set; }

    [Required] public bool IsPractice { get; set; }

    [Required] public int GroupId { get; set; }
    public Group? OwnerGroup { get; set; }


    public ICollection<Couple> Couples { get; set; } = new List<Couple>();
    public ICollection<HomeworkTask> Homework { get; set; } = new List<HomeworkTask>();
}