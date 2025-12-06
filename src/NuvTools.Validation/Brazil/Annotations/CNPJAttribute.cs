using NuvTools.Validation.Annotations;
using NuvTools.Validation.Resources;

namespace NuvTools.Validation.Brazil.Annotations;

/// <summary>
/// Validation attribute that validates Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica) numbers.
/// Validates the CNPJ checksum algorithm and accepts formatted (XX.XXX.XXX/XXXX-XX) or unformatted (XXXXXXXXXXXXXX) input.
/// </summary>
/// <example>
/// <code>
/// public class CompanyModel
/// {
///     [CNPJ]
///     public string CNPJ { get; set; }
/// }
/// </code>
/// </example>
public class CNPJAttribute : StringValueBaseAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CNPJAttribute"/> class.
    /// </summary>
    public CNPJAttribute()
        : base(() => Messages.XInvalid)
    {
    }

    /// <summary>
    /// Validates that the value is a valid Brazilian CNPJ number.
    /// </summary>
    /// <param name="value">The CNPJ value to validate.</param>
    /// <returns>True if the CNPJ is valid or if the value is null/empty; otherwise, false.</returns>
    public override bool IsValid(object? value)
    {
        // Automatically pass if value is null. RequiredAttribute should be used to assert a value is not null.
        if (!IsValidValue(value)) return true;

        string str = (string)value!;

        return Validator.IsCNPJ(str);
    }

}
