using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.Utilities.Attributes;

using static GCommon.EntityValidation.Skill;

public class ValidSkillNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string strValue)
            return ValidationResult.Success;

        string trimmedValue = strValue.Trim();

        if (string.IsNullOrEmpty(trimmedValue))
            return new ValidationResult($"Skill name cannot be empty or whitespace.");

        if (trimmedValue.Length < NameMinLength)
            return new ValidationResult($"Skill name must be at least {NameMinLength} characters long.");

        if (trimmedValue.Length > NameMaxLength)
            return new ValidationResult($"Skill name cannot exceed {NameMaxLength} characters.");

        return ValidationResult.Success;
    }
}
