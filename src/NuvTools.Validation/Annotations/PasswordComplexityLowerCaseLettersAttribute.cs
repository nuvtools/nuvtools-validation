using NuvTools.Validation.Resources;
using System.Text.RegularExpressions;

namespace NuvTools.Validation.Annotations;

public class PasswordComplexityLowerCaseLettersAttribute : PasswordComplexityBaseAttribute
{
    public PasswordComplexityLowerCaseLettersAttribute(int minOccurrences)
        : base(minOccurrences, () => Messages.XMustContainAtLeastYLowerCaseLetters)
    {
    }

    public override bool IsValid(object value)
    {
        EnsureLegalMinOcorrences();

        // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
        if (!IsValidValue(value)) return true;

        string str = value as string;

        return Regex.Matches(str, @"[a-z]").Count >= MinOccurrences;
    }
}
