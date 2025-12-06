using System.Text.RegularExpressions;

namespace NuvTools.Validation.Brazil;


/// <summary>
/// Provides validation methods for Brazilian document numbers and identification formats.
/// </summary>
public static class Validator
{
    /// <summary>
    /// Validates a CPF or CNPJ by automatically detecting the document type based on length.
    /// Accepts both formatted and unformatted input.
    /// </summary>
    /// <param name="value">The CPF or CNPJ value to validate (11 digits for CPF, 14 digits for CNPJ).</param>
    /// <returns>True if the value is a valid CPF or CNPJ; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool isValid1 = "123.456.789-01".IsCPForCNPJ(); // Validates as CPF
    /// bool isValid2 = "12.345.678/0001-95".IsCPForCNPJ(); // Validates as CNPJ
    /// </code>
    /// </example>
    public static bool IsCPForCNPJ(this string value)
    {
        string numbersOnly = value.GetNumbersOnly();

        if (numbersOnly.Length == 11)
            return IsCPF(numbersOnly);
        if (numbersOnly.Length == 14)
            return IsCNPJ(numbersOnly);
        return false;
    }

    /// <summary>
    /// Validates a Brazilian CPF (Cadastro de Pessoas Físicas) number using the official checksum algorithm.
    /// Accepts both formatted (XXX.XXX.XXX-XX) and unformatted (XXXXXXXXXXX) input.
    /// </summary>
    /// <param name="value">The CPF value to validate.</param>
    /// <returns>True if the CPF is valid according to the checksum algorithm; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool isValid = "123.456.789-01".IsCPF();
    /// bool isValidUnformatted = "12345678901".IsCPF();
    /// </code>
    /// </example>
    public static bool IsCPF(this string value)
    {
        value = value.GetNumbersOnly();

        if (value.Length != 11)
            return false;

        for (int j = 0; j < 10; j++)
            if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == value)
                return false;

        int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        string tempValue = value[..9];
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempValue[i].ToString()) * multiplier1[i];

        int remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        string digit = remainder.ToString();
        tempValue += digit;
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempValue[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        if (remainder < 2)
            remainder = 0;
        else
            remainder = 11 - remainder;

        digit += remainder.ToString();

        return value.EndsWith(digit);
    }

    /// <summary>
    /// Validates a Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica) number using the official checksum algorithm.
    /// Accepts both formatted (XX.XXX.XXX/XXXX-XX) and unformatted (XXXXXXXXXXXXXX) input.
    /// </summary>
    /// <param name="value">The CNPJ value to validate.</param>
    /// <returns>True if the CNPJ is valid according to the checksum algorithm; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool isValid = "12.345.678/0001-95".IsCNPJ();
    /// bool isValidUnformatted = "12345678000195".IsCNPJ();
    /// </code>
    /// </example>
    public static bool IsCNPJ(this string value)
    {
        value = value.GetNumbersOnly();

        if (value.Length != 14)
            return false;

        if (value.Distinct().Count() == 1)
            return false;

        int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempValue = value[..12];
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += (tempValue[i] - '0') * multiplier1[i];

        int remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        string digit = remainder.ToString();
        tempValue += digit;

        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += (tempValue[i] - '0') * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        digit += remainder.ToString();

        return value.EndsWith(digit);
    }

    /// <summary>
    /// Validates a Brazilian mobile phone number.
    /// The method checks if the phone number contains a valid Brazilian area code (DDD),
    /// followed by the digit 9 and then exactly eight digits (format: XX 9 XXXX-XXXX).
    /// </summary>
    /// <param name="mobileNumber">The mobile number to validate (accepts formatted or unformatted input).</param>
    /// <returns>True if the mobile number is valid; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool isValid = "11987654321".IsMobileNumber(); // São Paulo mobile
    /// bool isValid2 = "(21) 98765-4321".IsMobileNumber(); // Rio de Janeiro mobile
    /// </code>
    /// </example>
    public static bool IsMobileNumber(this string mobileNumber)
    {
        mobileNumber = mobileNumber.GetNumbersOnly();
        var regex = RegexPattern.MobileNumberRegex();
        var match = regex.Match(mobileNumber);
        return match.Success;
    }

    /// <summary>
    /// Validates a Brazilian ZIP code (CEP - Código de Endereçamento Postal).
    /// Accepts formats: XXXXX-XXX or XXXXXXXX (with or without hyphen).
    /// </summary>
    /// <param name="zipCode">The ZIP code to validate.</param>
    /// <returns>True if the ZIP code format is valid; otherwise, false.</returns>
    /// <example>
    /// <code>
    /// bool isValid = "01310-100".IsZipCodeNumber(); // With hyphen
    /// bool isValid2 = "01310100".IsZipCodeNumber(); // Without hyphen
    /// </code>
    /// </example>
    public static bool IsZipCodeNumber(this string zipCode)
    {
        return RegexPattern.ZipCodeRegex().IsMatch(zipCode);
    }
}