namespace NuvTools.Validation.Brazil;

/// <summary>
/// Provides formatting utilities for Brazilian document numbers.
/// </summary>
public static class Format
{
    /// <summary>
    /// Formats a CNPJ string into the standard Brazilian format: XX.XXX.XXX/XXXX-XX
    /// </summary>
    /// <param name="cnpj">The CNPJ number as a string (digits only).</param>
    /// <returns>The formatted CNPJ string.</returns>
    /// <example>
    /// <code>
    /// string formatted = "12345678000195".FormatCNPJ();
    /// // Returns: "12.345.678/0001-95"
    /// </code>
    /// </example>
    public static string FormatCNPJ(this string cnpj)
    {
        return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
    }

    /// <summary>
    /// Formats a CPF string into the standard Brazilian format: XXX.XXX.XXX-XX
    /// </summary>
    /// <param name="cpf">The CPF number as a string (digits only).</param>
    /// <returns>The formatted CPF string.</returns>
    /// <example>
    /// <code>
    /// string formatted = "12345678901".FormatCPF();
    /// // Returns: "123.456.789-01"
    /// </code>
    /// </example>
    public static string FormatCPF(this string cpf)
    {
        return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
    }
}
