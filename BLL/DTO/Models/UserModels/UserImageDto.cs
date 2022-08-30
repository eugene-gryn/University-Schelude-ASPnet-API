using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.DTO.Models.UserModels;

public class UserImageDto : IDisposable {
    [MaxLength(500_000)] public MemoryStream ProfileImage { get; set; } = null!;


    [Required]
    [StringLength(20, MinimumLength = 5)]
    [Column(TypeName = "VARCHAR")]
    public string ContentType { get; set; }

    public void Dispose() {
        ProfileImage.Dispose();
    }
}