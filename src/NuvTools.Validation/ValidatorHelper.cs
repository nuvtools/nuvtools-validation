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
}