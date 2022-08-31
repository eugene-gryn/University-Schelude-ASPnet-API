using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.DTO.Models.UserModels;

public class UserImageDto : IDisposable {
    [MaxLength(500_000)] public MemoryStream Image { get; set; } = null!;


    [Required]
    [StringLength(20, MinimumLength = 5)]
    public string ContentType { get; set; } = null!; 

    public void Dispose() {
        Image.Dispose();
    }
}