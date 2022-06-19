using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Couple
{
    [Key] public int Id { get; set; }

    [Required] public DateTime Begin { get; set; }

    [Required] public DateTime End { get; set; }

    [Required] public Subject Subject { get; set; }
}