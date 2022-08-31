using System.ComponentModel.DataAnnotations;
using BLL.DTO.Models.SubjectModels;
using BLL.DTO.Models.UserModels;

namespace BLL.DTO.Models.HomeworkTaskModels;

public class HomeworkTaskDto {
    [Key] public int Id { get; set; }

    [MaxLength(1000)] public string? Description { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Deadline { get; set; }

    [Required] public byte Priority { get; set; } = 5;

    [Required] public int UserId { get; set; }

    [Required] public int SubjectId { get; set; }
}