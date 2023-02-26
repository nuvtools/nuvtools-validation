using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NuvTools.Validation;

/// <summary>
/// Validators's class.
/// </summary>
public static class Validator
{
    /// <summary>
    /// Validate the e-mail address.
    /// </summary>
    /// <param name="value">E-mail address</param>
    /// <returns></returns>
    public static bool IsEmail(this string value) => Regex.IsMatch(value, RegexPattern.EmailAddress);

    public static List<string> Validate<T>(this T obj)
    {
        ValidationContext context = new(obj, serviceProvider: null, items: null);
        List<ValidationResult> results = new();
        bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, context, results, true);

        if (isValid == false)
        {
            List<string> errors = new();
            foreach (var validationResult in results)
                errors.Add(validationResult.ErrorMessage);

            return errors;
        }

        return null;
    }
}