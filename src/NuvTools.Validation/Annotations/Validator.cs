using System.ComponentModel.DataAnnotations;

namespace NuvTools.Validation.Annotations;

/// <summary>
/// Provides validation methods for objects decorated with data annotation attributes.
/// </summary>
public static class Validator
{
    /// <summary>
    /// Validates an object using its data annotation attributes.
    /// </summary>
    /// <typeparam name="T">The type of the object to validate.</typeparam>
    /// <param name="obj">The object to validate.</param>
    /// <returns>A list of error messages if validation fails; otherwise, null if the object is valid.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="obj"/> is null.</exception>
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