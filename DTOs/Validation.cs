using System.ComponentModel.DataAnnotations;

namespace ClubHub.DTOs;

public class GmailOnlyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var email = value as string;
        if (string.IsNullOrWhiteSpace(email)) return new ValidationResult("Email is required.");
        var normalizedEmail = email.Trim().ToLowerInvariant();
        if (!normalizedEmail.EndsWith("@gmail.com")) return new ValidationResult("Email must end with @gmail.com");
        return ValidationResult.Success;
    }
}

public class CountryCodeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var phone = value as string;
        if (string.IsNullOrWhiteSpace(phone)) return new ValidationResult("Phone number is required.");
        var normalizedPhone = phone.Trim();
        if (!normalizedPhone.StartsWith("+947")) return new ValidationResult("Phone number must start with +947");
        if (normalizedPhone.Length != 12) return new ValidationResult("Phone number must be 12 characters (example: +94771234567)");
        
        foreach (var character in normalizedPhone.Substring(1))
        {
            if (!char.IsDigit(character)) return new ValidationResult("Phone number must contain only digits after the + sign");
        }
        return ValidationResult.Success;
    }
}

public class PasswordStrengthAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var password = value as string;
        if (string.IsNullOrWhiteSpace(password)) return new ValidationResult("Password is required.");
        bool hasLetter = password.Any(char.IsLetter);
        bool hasDigit = password.Any(char.IsDigit);
        if (!hasLetter || !hasDigit) return new ValidationResult("Password must contain at least one letter and one number.");
        return ValidationResult.Success;
    }
}

public class ValidNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var name = value as string;
        if (string.IsNullOrWhiteSpace(name)) return new ValidationResult("Name is required.");
        
        if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
        {
            return new ValidationResult("Name can only contain letters and spaces.");
        }
        
        return ValidationResult.Success;
    }
}