using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class UserRole
{
    public int UserId { get; set; }
    public User User { get; set; } = new();
    public int GroupId { get; set; }
    public Group Group { get; set; } = new();

    public bool IsModerator { get; set; } = false;

    public bool IsOwner { get; set; } = false;
}