using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace DAL.Entities;

public class Couple
{
    [Key] public int Id { get; set; }

    [Required, DataType(DataType.DateTime)] public DateTime Begin { get; set; }

    [Required] public DateTime End { get; set; }

    [Required] public Subject Subject { get; set; } = new();
}