using System.Globalization;

namespace NuvTools.Validation.Brazil;

/// <summary>
/// Validators's class.
/// </summary>
public static class Validator
{
    /// <summary>
    /// Método para validar um CPF ou um CNPJ. 
    /// </summary>
    /// <param name="value">Informe um CPF ou um CNPJ. Retorno True: CPF ou CNPJ válido. False: CPF ou CNPJ inválido.</param>
    /// <returns>True: CPF ou CNPJ válido. False: CPF ou CNPJ inválido.</returns>
    public static bool IsCPForCNPJ(string value)
    {
        string numbersOnly = value.GetNumbersOnly();

        if (numbersOnly.Length == 11)
            return IsCPF(numbersOnly);
        if (numbersOnly.Length == 14)
            return IsCNPJ(numbersOnly);
        return false;
    }

    /// <summary>
    /// Método para validar um CPF. 
    /// </summary>
    /// <param name="value">Informe um CPF. Retorno True: CPF válido. False: CPF inválido.</param>
    /// <returns>True: CPF válido. False: CPF inválido.</returns>
    public static bool IsCPF(string value)
    {
        string numbersOnly = value.GetNumbersOnly();

        if (numbersOnly.Length != 11)
            return false;

        string number = numbersOnly.Substring(0, 9);
        string verifiedNumber = numbersOnly.Substring(9, 2);
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(number.Substring(i, 1), NumberFormatInfo.InvariantInfo) * (10 - i);

        if (sum == 0)
            return false;

        sum = 11 - sum % 11;

        if (sum > 9)
            sum = 0;

        if (int.Parse(verifiedNumber.Substring(0, 1), NumberFormatInfo.InvariantInfo) != sum)
            return false;

        sum *= 2;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(number.Substring(i, 1), NumberFormatInfo.InvariantInfo) * (11 - i);

        sum = 11 - sum % 11;
        if (sum > 9)
            sum = 0;
        if (int.Parse(verifiedNumber.Substring(1, 1), NumberFormatInfo.InvariantInfo) != sum)
            return false;

        return true;
    }

    /// <summary>
    /// Método para validar um CNPJ. 
    /// </summary>
    /// <param name="value">Informe um CNPJ. Retorno True: CNPJ válido. False: CNPJ inválido.</param>
    /// <returns>True: CNPJ válido. False: CNPJ inválido.</returns>
    public static bool IsCNPJ(string value)
    {
        string numbersOnly = value.GetNumbersOnly();

        if (numbersOnly.Length != 14)
            return false;

        string number = numbersOnly.Substring(0, 12);
        string verifiedNumber = numbersOnly.Substring(12, 2);
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(number.Substring(11 - i, 1), NumberFormatInfo.InvariantInfo) * (2 + i % 8);

        if (sum == 0)
            return false;

        sum = 11 - sum % 11;
        if (sum > 9)
            sum = 0;

        if (int.Parse(verifiedNumber.Substring(0, 1), NumberFormatInfo.InvariantInfo) != sum)
            return false;

        sum *= 2;
        for (int i = 0; i < 12; i++)
            sum += int.Parse(number.Substring(11 - i, 1), NumberFormatInfo.InvariantInfo) * (2 + (i + 1) % 8);

        sum = 11 - sum % 11;
        if (sum > 9)
            sum = 0;

        if (int.Parse(verifiedNumber.Substring(1, 1), NumberFormatInfo.InvariantInfo) != sum)
            return false;

        return true;
    }

}