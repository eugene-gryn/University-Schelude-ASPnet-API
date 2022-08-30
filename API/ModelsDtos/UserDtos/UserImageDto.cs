using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.ModelsDtos.UserDtos;

public class UserImageDto {

    [MaxLength(500_000)] public string ProfileImage { get; set; } = null!;


    [Required]
    [StringLength(20, MinimumLength = 5)]
    [Column(TypeName = "VARCHAR")]
    public string ContentType { get; set; } = null!;
}