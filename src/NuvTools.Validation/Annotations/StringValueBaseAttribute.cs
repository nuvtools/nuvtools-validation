using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NuvTools.Validation.Annotations;

public class StringValueBaseAttribute : ValidationAttribute
{
    public StringValueBaseAttribute(Func<string> errorMessageAccessor)
        : base(errorMessageAccessor)
    {
    }

    /// <summary>
    /// Applies formatting to a specified error message. (Overrides <see cref = "ValidationAttribute.FormatErrorMessage" />)
    /// </summary>
    /// <param name="name">The name to include in the formatted string.</param>
    /// <returns>A localized string to describe the minimal occurrences of capital letters.</returns>
    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
    }

    protected static bool IsValidValue(object value)
    {
        return value != null
            && value is string
            && !string.IsNullOrWhiteSpace(value.ToString());
    }

}
