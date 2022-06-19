using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Subject
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(50)] public string Name { get; set; }

    [Required] public bool IsPractice { get; set; }


    public string? GoogleMeetUrl { get; set; }
}