using NuvTools.Validation.Resources;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NuvTools.Validation.Annotations;

/// <summary>
/// Abstract base class for password complexity validation attributes.
/// Provides common functionality for validating minimum occurrences of specific character types in passwords.
/// </summary>
/// <param name="minOccurrences">The minimum number of occurrences required for the specific character type.</param>
/// <param name="errorMessageAccessor">A function that returns the error message to use when validation fails.</param>
public abstract class PasswordComplexityBaseAttribute(int minOccurrences, Func<string> errorMessageAccessor) : ValidationAttribute(errorMessageAccessor)
{
    /// <summary>
    /// Gets the minimum number of occurrences required for the specific character type.
    /// </summary>
    public int MinOccurrences { get; private set; } = minOccurrences;

    /// <summary>
    /// Applies formatting to a specified error message. (Overrides <see cref = "ValidationAttribute.FormatErrorMessage" />)
    /// </summary>
    /// <param name="name">The name to include in the formatted string.</param>
    /// <returns>A localized string to describe the minimal occurrences of capital letters.</returns>
    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinOccurrences);
    }

    /// <summary>
    /// Checks that MinOccurrences has a legal value.
    /// </summary>
    /// <exception cref="InvalidOperationException">MinOccurrences is zero or less than negative one.</exception>
    protected void EnsureLegalMinOcorrences()
    {
        if (MinOccurrences == 0 || MinOccurrences < -1)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Messages.InvalidValue, GetType().Name));
        }
    }

    /// <summary>
    /// Determines whether the provided value is a valid non-empty string for password validation.
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
