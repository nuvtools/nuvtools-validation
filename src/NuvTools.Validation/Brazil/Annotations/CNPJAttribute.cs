using NuvTools.Validation.Annotations;
using NuvTools.Validation.Resources;

namespace NuvTools.Validation.Brazil.Annotations;

public class CNPJAttribute : StringValueBaseAttribute
{
    public CNPJAttribute()
        : base(() => Messages.XInvalid)
    {
    }

    public override bool IsValid(object? value)
    {
        // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
        if (!IsValidValue(value)) return true;

        string str = (string)value!;

        return Validator.IsCNPJ(str);
    }

}
