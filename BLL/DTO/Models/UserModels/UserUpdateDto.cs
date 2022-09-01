using System.ComponentModel.DataAnnotations;
using BLL.DTO.Models.UserModels.Attributes;

namespace BLL.DTO.Models.UserModels; 

public class UserUpdateDto {
    [StringLength(20, MinimumLength = 4)]
    [Login]
    public string? Name { get; set; }

    [StringLength(15, MinimumLength = 1)]
    [TelegramToken]
    public string? TelegramToken { get; set; }

    public SettingsDto? Settings { get; set; }
}