using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NuvTools.Validation.Annotations;

/// <summary>
/// Base class for validation attributes that validate string values.
/// Provides common functionality for string-based validation scenarios.
/// </summary>
/// <param name="errorMessageAccessor">A function that returns the error message to use when validation fails.</param>
public class StringValueBaseAttribute(Func<string> errorMessageAccessor) : ValidationAttribute(errorMessageAccessor)
{

    /// <summary>
    /// Applies formatting to a specified error message. (Overrides <see cref = "ValidationAttribute.FormatErrorMessage" />)
    /// </summary>
    /// <param name="name">The name to include in the formatted string.</param>
    /// <returns>A localized string to describe the validation requirement.</returns>
    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
    }

    /// <summary>
    /// Determines whether the provided value is a valid non-empty string.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is a non-null, non-empty string; otherwise, false.</returns>
    protected static bool IsValidValue(object? value)
    {
        return value != null
            && value is string
            && !string.IsNullOrWhiteSpace(value.ToString());
    }

}
