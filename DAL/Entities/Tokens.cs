using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class Tokens {

    [Column(TypeName = "VARCHAR")]
    [StringLength(64)]
    public string RefreshToken { get; set; } = String.Empty;

    [DataType(DataType.DateTime)]
    public DateTime TokenCreated { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime TokenExpires { get; set; } = DateTime.UtcNow;
}