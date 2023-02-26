using NuvTools.Validation.Resources;
using System.Text.RegularExpressions;

namespace NuvTools.Validation.Annotations;

public class PasswordComplexityCapitalLettersAttribute : PasswordComplexityBaseAttribute
{
    public PasswordComplexityCapitalLettersAttribute(int minOccurrences)
        : base(minOccurrences, () => Messages.XMustContainAtLeastYCapitalLetters)
    {
    }

    public override bool IsValid(object value)
    {
        EnsureLegalMinOcorrences();

        // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
        if (!IsValidValue(value)) return true;

        string str = value as string;

        return Regex.Matches(str, @"[A-Z]").Count >= MinOccurrences;
    }

}
