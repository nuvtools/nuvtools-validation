using NuvTools.Validation.Resources;
using System.Text.RegularExpressions;

namespace NuvTools.Validation.Annotations;

public class PasswordComplexityDigitsAttribute : PasswordComplexityBaseAttribute
{
    public PasswordComplexityDigitsAttribute(int minOccurrences)
         : base(minOccurrences, () => Messages.XMustContainAtLeastYDigits)
    {
    }

    public override bool IsValid(object value)
    {
        EnsureLegalMinOcorrences();

        // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
        if (!IsValidValue(value)) return true;

        string str = value as string;

        return Regex.Matches(str, @"[0-9]").Count >= MinOccurrences;
    }
}
