namespace NuvTools.Validation.Brazil;

public static class Format
{
    public static string FormatCNPJ(this string cnpj)
    {
        return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
    }
    public static string FormatCPF(this string cpf)
    {
        return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
    }
}
