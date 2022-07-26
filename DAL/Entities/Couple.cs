using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace DAL.Entities;

public class Couple
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key] public int Id { get; set; }

    [Required, DataType(DataType.DateTime)] public DateTime Begin { get; set; }

    [Required, DataType(DataType.DateTime)] public DateTime End { get; set; }

    [Required] public Subject Subject { get; set; } = new();
}