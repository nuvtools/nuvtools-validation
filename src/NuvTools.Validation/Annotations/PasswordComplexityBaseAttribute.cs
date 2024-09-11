using NuvTools.Validation.Resources;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NuvTools.Validation.Annotations;

public abstract class PasswordComplexityBaseAttribute(int minOccurrences, Func<string> errorMessageAccessor) : ValidationAttribute(errorMessageAccessor)
{
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

    protected static bool IsValidValue(object? value)
    {
        return value != null
            && value is string
            && !string.IsNullOrWhiteSpace(value.ToString());
    }

}
