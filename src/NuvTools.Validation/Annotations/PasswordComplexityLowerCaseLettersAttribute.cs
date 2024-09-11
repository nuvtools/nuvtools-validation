using NuvTools.Validation.Resources;
using System.Text.RegularExpressions;

namespace NuvTools.Validation.Annotations;

public class PasswordComplexityLowerCaseLettersAttribute(int minOccurrences) : PasswordComplexityBaseAttribute(minOccurrences, () => Messages.XMustContainAtLeastYLowerCaseLetters)
{
    public override bool IsValid(object? value)
    {
        EnsureLegalMinOcorrences();

        // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
        if (!IsValidValue(value)) return true;

        string str = (string)value!;

        return Regex.Matches(str, @"[a-z]").Count >= MinOccurrences;
    }
}
