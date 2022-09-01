using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BLL.DTO.Models.UserModels.Attributes;

public class LoginAttribute : ValidationAttribute {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        if (value != null) {
            string val = value.ToString()!;
            if (string.IsNullOrEmpty(val)) return new ValidationResult("Empty string provided!");

            if (Char.IsDigit(val.First())) return new ValidationResult("String must not starts with number");

            if (!new Regex(@"^(\w+)\z").IsMatch(val))
                return new ValidationResult("String must does not contain special symbols");

            return ValidationResult.Success;
        }
        
        return ValidationResult.Success;
    }
}