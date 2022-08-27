using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BLL.DTO.Models.UserModels.Attributes;

public class PasswordAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var hasANum = new Regex(@"\d");
        var hasAchars = new Regex(@"\w");

        if (value != null)
        {
            string val = value.ToString()!;
            if (string.IsNullOrEmpty(val)) return new ValidationResult("Empty string provided!");

            if(!(hasANum.IsMatch(val) && hasAchars.IsMatch(val))) return new ValidationResult("String must have at least one char and digit!");

            return ValidationResult.Success;
        }

        return new ValidationResult("Value is empty");
    }
}