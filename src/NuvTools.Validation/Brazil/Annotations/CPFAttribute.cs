using NuvTools.Validation.Annotations;
using NuvTools.Validation.Resources;

namespace NuvTools.Validation.Brazil.Annotations;

/// <summary>
/// Validation attribute that validates Brazilian CPF (Cadastro de Pessoas Físicas) numbers.
/// Validates the CPF checksum algorithm and accepts formatted (XXX.XXX.XXX-XX) or unformatted (XXXXXXXXXXX) input.
/// </summary>
/// <example>
/// <code>
/// public class PersonModel
/// {
///     [CPF]
///     public string CPF { get; set; }
/// }
/// </code>
/// </example>
public class CPFAttribute : StringValueBaseAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CPFAttribute"/> class.
    /// </summary>
    public CPFAttribute()
        : base(() => Messages.XInvalid)
    {
    }

    /// <summary>
    /// Validates that the value is a valid Brazilian CPF number.
    /// </summary>
    /// <param name="value">The CPF value to validate.</param>
    /// <returns>True if the CPF is valid or if the value is null/empty; otherwise, false.</returns>
    public override bool IsValid(object? value)
    {
        // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
        if (!IsValidValue(value)) return true;

        string str = (string)value!;

        return Validator.IsCPF(str);
    }

}
