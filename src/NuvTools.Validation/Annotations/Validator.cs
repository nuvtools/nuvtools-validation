using System.ComponentModel.DataAnnotations;

namespace NuvTools.Validation.Annotations;

/// <summary>
/// Validators's class.
/// </summary>
public static class Validator
{
    public static List<string>? Validate<T>(this T obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        ValidationContext context = new(obj, serviceProvider: null, items: null);
        List<ValidationResult> results = [];
        bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, context, results, true);

        if (isValid) return null;

        List<string> errors = [];
        foreach (var validationResult in results)
            errors.Add(validationResult.ErrorMessage!);

        return errors;
    }
}