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

    /// <summary>
    /// Checks if the provided string contains only numeric characters.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>True if the input contains only numeric characters, otherwise false.</returns>
    public static bool HasNumbersOnly(this string input)
    {
        // Verifies if the string is null or empty
        if (string.IsNullOrEmpty(input)) return false;

        // Checks if all characters in the string are digits
        return input.All(char.IsDigit);
    }

    /// <summary>
    /// Checks if the provided string represents a long type number.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <param name="positiveOnly">If only positive numbers is valid.</param>
    /// <returns>True if the input is a whole number, otherwise false.</returns>
    public static bool IsIntNumber(this string input, bool positiveOnly = false)
    {
        // Try to parse the input string as int
        if (int.TryParse(input, out int number))
            return positiveOnly ? int.IsPositive(number) : true;

        // Return false if parsing fails or number is negative
        return false;
    }

    /// <summary>
    /// Checks if the provided string represents a long type number.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <param name="positiveOnly">If only positive numbers is valid.</param>
    /// <returns>True if the input is a whole number, otherwise false.</returns>
    public static bool IsLongNumber(this string input, bool positiveOnly = false)
    {
        // Try to parse the input string as long
        if (long.TryParse(input, out long number))
            return positiveOnly ? long.IsPositive(number) : true;

        // Return false if parsing fails or number is negative
        return false;
    }

    /// <summary>
    /// Checks if the provided string represents a valid decimal number.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <param name="positiveOnly">If only positive numbers is valid.</param>
    /// <param name="provider">An object that supplies culture-specific parsing information about s.</param>
    /// <returns>True if the input is a valid decimal number, otherwise false.</returns>
    public static bool IsDecimalNumber(this string input, bool positiveOnly = false, IFormatProvider provider = null)
    {
        // Try to parse the input string as a decimal
        if (decimal.TryParse(input, System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowDecimalPoint, provider, out decimal number))
            return positiveOnly ? decimal.IsPositive(number) : true;

        return false;
    }
}