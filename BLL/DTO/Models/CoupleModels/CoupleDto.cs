using System.ComponentModel.DataAnnotations;
using BLL.DTO.Models.GroupsModels;
using BLL.DTO.Models.SubjectModels;

namespace BLL.DTO.Models.CoupleModels;

public class CoupleDto {
    [Key] public int Id { get; set; }

    [Required] public DateTime Begin { get; set; }

    [Required] public DateTime End { get; set; }

    [Required] public int SubjectId { get; set; }
    public SubjectDto? Subject { get; set; }

    [Required] public int GroupId { get; set; }
    public GroupDto? Group { get; set; }
}