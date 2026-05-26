using System.Text.RegularExpressions;

namespace NuvTools.Validation;

/// <summary>
/// Helpers to works with validation functions.
/// </summary>
internal static partial class ValidatorHelper
{

    public static string GetNumbersOnly(this string value)
    {
        return GetNumbersOnlyRegex().Replace(value, string.Empty);
    }

    [GeneratedRegex(@"\D", RegexOptions.IgnoreCase)]
    private static partial Regex GetNumbersOnlyRegex();

    public static string GetAlphanumericOnly(this string value)
    {
        return GetAlphanumericOnlyRegex().Replace(value, string.Empty).ToUpperInvariant();
    }

    [GeneratedRegex(@"[^0-9A-Za-z]")]
    private static partial Regex GetAlphanumericOnlyRegex();
}