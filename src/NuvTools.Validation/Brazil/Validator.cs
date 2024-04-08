namespace NuvTools.Validation.Brazil;

/// <summary>
/// Validators's class.
/// </summary>
public static class Validator
{
    /// <summary>
    /// Validates a CPF or CNPJ. 
    /// </summary>
    /// <param name="value">Enter a CPF or a CNPJ.</param>
    /// <returns>True: CPF or CNPJ is valid. False: CPF or CNPJ is invalid.</returns>
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
    /// Validates a CPF.
    /// </summary>
    /// <param name="value">Enter a CPF.</param>
    /// <returns>True: CPF or CNPJ is valid. False: CPF or CNPJ is invalid.</returns>
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
    /// Método para validar um CNPJ. 
    /// </summary>
    /// <param name="value">Informe um CNPJ. Retorno True: CNPJ válido. False: CNPJ inválido.</param>
    /// <returns>True: CNPJ válido. False: CNPJ inválido.</returns>
    public static bool IsCNPJ(this string value)
    {
        value = value.GetNumbersOnly();

        if (value.Length != 14)
            return false;

        int[] multiplier1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplier2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        string tempValue = value[..12];
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempValue[i].ToString()) * multiplier1[i];

        int remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        string digit = remainder.ToString();
        tempValue += digit;

        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempValue[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        digit += remainder.ToString();

        return value.EndsWith(digit);
    }

}