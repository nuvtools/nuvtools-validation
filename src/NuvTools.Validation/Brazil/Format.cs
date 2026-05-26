namespace NuvTools.Validation.Brazil;

/// <summary>
/// Provides formatting utilities for Brazilian document numbers.
/// </summary>
public static class Format
{
    /// <summary>
    /// Formats a CNPJ string into the standard Brazilian format: XX.XXX.XXX/XXXX-XX.
    /// Accepts both legacy numeric and the new RFB alphanumeric (uppercase A–Z and 0–9) representations.
    /// Separators in the input are stripped; lowercase letters are normalized to uppercase.
    /// </summary>
    /// <param name="cnpj">The CNPJ number as a string (14 alphanumeric characters, with or without separators).</param>
    /// <returns>The formatted CNPJ string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="cnpj"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when the input does not contain exactly 14 alphanumeric characters.</exception>
    /// <example>
    /// <code>
    /// string formatted = "12345678000195".FormatCNPJ();
    /// // Returns: "12.345.678/0001-95"
    /// string alphanumeric = "12ABC34501DE35".FormatCNPJ();
    /// // Returns: "12.ABC.345/01DE-35"
    /// </code>
    /// </example>
    public static string FormatCNPJ(this string cnpj)
    {
        ArgumentNullException.ThrowIfNull(cnpj);

        string raw = cnpj.GetAlphanumericOnly();
        if (raw.Length != 14)
            throw new FormatException("CNPJ must contain exactly 14 alphanumeric characters.");

        return $"{raw[..2]}.{raw.Substring(2, 3)}.{raw.Substring(5, 3)}/{raw.Substring(8, 4)}-{raw.Substring(12, 2)}";
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
