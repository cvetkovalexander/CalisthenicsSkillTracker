using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.GCommon.Utilities.Attributes;

public class NotFutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime dateTime)
        {
            if (dateTime > DateTime.Now)
            {
                return new ValidationResult("Date cannot be in the future.");
            }
        }

        return ValidationResult.Success;
    }
}
