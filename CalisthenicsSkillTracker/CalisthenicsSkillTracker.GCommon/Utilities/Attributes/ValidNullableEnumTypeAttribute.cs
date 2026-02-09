using System.ComponentModel.DataAnnotations;

namespace CalisthenicsSkillTracker.Utilities.Attributes;

public class ValidNullableEnumTypeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var type = value.GetType();

        // Handle nullable enums
        // If the type is nullable, get the underlying enum type
        // If not nullable, it will return the original type, which will be checked for being an enum

        type = Nullable.GetUnderlyingType(type) ?? type;

        if (!type.IsEnum)
            return new ValidationResult("ValidEnumAttribute can only be used on enum properties.");

        if (!Enum.IsDefined(type, value))
            return new ValidationResult(
                ErrorMessage ?? $"{value} is not a valid value."
            );

        return ValidationResult.Success;
    }
}
