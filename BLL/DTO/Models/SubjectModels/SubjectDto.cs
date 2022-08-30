using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.DTO.Models.CoupleModels;
using BLL.DTO.Models.GroupsModels;
using BLL.DTO.Models.HomeworkTaskModels;
using DAL.Entities;

namespace BLL.DTO.Models.SubjectModels;

public class SubjectDto {
    [Key] public int Id { get; set; }

    [Required] [StringLength(50)] public string Name { get; set; } = string.Empty;

    [DataType(DataType.Url)]
    [StringLength(200)]
    public string? Url { get; set; }

    [StringLength(50)] public string? Location { get; set; }

    [StringLength(50)] public string? Teacher { get; set; }

    [Required] public bool IsPractice { get; set; }

    [Required] public int GroupId { get; set; }
    public GroupDto? OwnerGroup { get; set; };


    public ICollection<CoupleDto> Couples { get; set; } = new List<CoupleDto>();
    public ICollection<HomeworkTaskDto> Homework { get; set; } = new List<HomeworkTaskDto>();
}