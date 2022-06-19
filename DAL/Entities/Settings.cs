using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class Settings
{
    [Required] public bool NotifyBeforeCouple { get; set; }

    [Required] public bool NotifyAboutCouple { get; set; }

    [Required] public bool NotifyAboutHomework { get; set; }

    [Required] public bool NotifyAboutDeadlineHomework { get; set; }

    [Required] public bool NotifyAboutLoseDeadlineHomework { get; set; }
}