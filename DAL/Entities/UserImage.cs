using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

public class UserImage {
    [Required] [MaxLength(500_000)] public byte[] ProfileImage { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 5)]
    [Column(TypeName = "VARCHAR")]
    public string ContentType { get; set; }
}