using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Settings
{
    public bool NotifyBeforeCouple { get; set; } = true;

    public bool NotifyAboutCouple { get; set; } = true;

    public bool NotifyAboutHomework { get; set; } = true;

    public bool NotifyAboutDeadlineHomework { get; set; } = true;

    public bool NotifyAboutLoseDeadlineHomework { get; set; } = true;
}