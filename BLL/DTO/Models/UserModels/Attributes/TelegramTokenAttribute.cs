using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BLL.DTO.Models.UserModels.Attributes;
public class TelegramTokenAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var token = new Regex(@"^(\d+)\z");

        if (value != null) {
            var val = value.ToString();

            if (val != null && !token.IsMatch(val)) return new ValidationResult("Token must have only numbers!");

            return ValidationResult.Success;
        }

        return ValidationResult.Success;
    }
}