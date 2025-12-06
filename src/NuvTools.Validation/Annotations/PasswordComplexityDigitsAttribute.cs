using NuvTools.Validation.Resources;
using System.Text.RegularExpressions;

namespace NuvTools.Validation.Annotations;

/// <summary>
/// Validation attribute that ensures a password contains a minimum number of digits (0-9).
/// </summary>
/// <param name="minOccurrences">The minimum number of digits required in the password.</param>
/// <example>
/// <code>
/// public class UserModel
/// {
///     [PasswordComplexityDigits(2)]
///     public string Password { get; set; }
/// }
/// </code>
/// </example>
public class PasswordComplexityDigitsAttribute(int minOccurrences) : PasswordComplexityBaseAttribute(minOccurrences, () => Messages.XMustContainAtLeastYDigits)
{
    /// <summary>
    /// Validates that the value contains at least the minimum number of digits.
    /// </summary>
    /// <param name="value">The password value to validate.</param>
    /// <returns>True if the password contains enough digits or if the value is null/empty; otherwise, false.</returns>
    public override bool IsValid(object? value)
    {
        EnsureLegalMinOcorrences();

        // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
        if (!IsValidValue(value)) return true;

        string str = (string)value!;

        return Regex.Matches(str, @"[0-9]").Count >= MinOccurrences;
    }
}
