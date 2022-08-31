namespace BLL.DTO.Models.UserModels; 

public class SettingsDto {
    public bool NotifyBeforeCouple { get; set; } = true;

    public bool NotifyAboutCouple { get; set; } = true;

    public bool NotifyAboutHomework { get; set; } = true;

    public bool NotifyAboutDeadlineHomework { get; set; } = true;

    public bool NotifyAboutLoseDeadlineHomework { get; set; } = true;
}