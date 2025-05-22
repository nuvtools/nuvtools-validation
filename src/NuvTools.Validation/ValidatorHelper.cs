using System.Text.RegularExpressions;

namespace NuvTools.Validation;

/// <summary>
/// Helpers to works with validation functions.
/// </summary>
internal static class ValidatorHelper
{

    public static string GetNumbersOnly(this string value)
    {
        return new Regex(@"\D", RegexOptions.IgnoreCase).Replace(value, string.Empty);
    }
}