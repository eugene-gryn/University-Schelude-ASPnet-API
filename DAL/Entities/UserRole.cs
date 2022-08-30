using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class UserRole
{
    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }
    
    [Required]
    public int GroupId { get; set; }
    public Group? Group { get; set; }
    
    [Required]
    public bool IsModerator { get; set; } = false;

    [Required]
    public bool IsOwner { get; set; } = false;
}