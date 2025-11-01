using NUnit.Framework;
using NuvTools.Validation.Brazil;

namespace NuvTools.Validation.Tests.Brazil;

[TestFixture()]
public class BrazilValidatorExtensions
{
    [Test()]
    public void ValidateCNPJ()
    {
        Assert.That(!"11111111111111".IsCNPJ());
        Assert.That(!"00000000000000".IsCNPJ());
        Assert.That("03.785.417/0001-03".IsCNPJ());
        Assert.That("54243121000193".IsCNPJ());

        // Additional tests
        Assert.That(!"12345678901234".IsCNPJ()); // Invalid CNPJ
        Assert.That(!"".IsCNPJ()); // Empty string
        Assert.That(!"03.785.417/0001-00".IsCNPJ()); // Valid format, invalid digits
        Assert.That(!"03.785.417/0001-031".IsCNPJ()); // Too long
        Assert.That(!"03.785.417/0001".IsCNPJ()); // Too short
    }

    [Test()]
    public void ValidateCPF()
    {
        Assert.That(!"11111111111".IsCPF());
        Assert.That(!"22222222222".IsCPF());
        Assert.That(!"erro".IsCPF());
        Assert.That("583.008.930-08".IsCPF());
        Assert.That("83289988074".IsCPF());

        // Additional tests
        Assert.That(!"12345678901".IsCPF()); // Invalid CPF
        Assert.That(!"".IsCPF()); // Empty string
        Assert.That(!"583.008.930-00".IsCPF()); // Valid format, invalid digits
        Assert.That(!"583.008.930-081".IsCPF()); // Too long
        Assert.That(!"583.008.930".IsCPF()); // Too short
    }

    [Test()]
    public void ValidateMobileNumber()
    {
        Assert.That("61944446666".IsMobileNumber());
        Assert.That("21955557777".IsMobileNumber());
        Assert.That("(21) 95555-7777".IsMobileNumber());
        Assert.That(!"erro".IsMobileNumber());
        Assert.That(!"994645".IsMobileNumber());
        Assert.That(!"21866664444".IsMobileNumber());

        // Additional tests
        Assert.That(!"".IsMobileNumber()); // Empty string
        Assert.That(!"1234567890".IsMobileNumber()); // Too short
        Assert.That(!"999999999999".IsMobileNumber()); // Too long
        Assert.That(!"1199999999a".IsMobileNumber()); // Contains letter
        Assert.That("11999999999".IsMobileNumber()); // Valid São Paulo mobile
    }

    [Test()]
    public void ValidateZipCode()
    {
        // Valid ZIP codes
        Assert.That("71065-100".IsZipCodeNumber(), Is.True);
        Assert.That("88999232".IsZipCodeNumber(), Is.True);

        // Invalid ZIP codes
        Assert.That("(21) 95555-7777".IsZipCodeNumber(), Is.False);
        Assert.That("95.555-777".IsZipCodeNumber(), Is.False);
        Assert.That("error".IsZipCodeNumber(), Is.False);

        // Additional tests
        Assert.That(!"".IsZipCodeNumber()); // Empty string
        Assert.That(!"1234567".IsZipCodeNumber()); // Too short
        Assert.That(!"123456789".IsZipCodeNumber()); // Too long
        Assert.That(!"12345-6789".IsZipCodeNumber()); // US format
        Assert.That("12345-678".IsZipCodeNumber(), Is.True); // Valid Brazilian format
    }

    [Test()]
    public void ValidateCPForCNPJ()
    {
        // Valid CPF
        Assert.That("583.008.930-08".IsCPForCNPJ(), Is.True);
        // Valid CNPJ
        Assert.That("03.785.417/0001-03".IsCPForCNPJ(), Is.True);

        // Invalid cases
        Assert.That(!"11111111111".IsCPForCNPJ());
        Assert.That(!"11111111111111".IsCPForCNPJ());
        Assert.That(!"erro".IsCPForCNPJ());
        Assert.That(!"1234567890".IsCPForCNPJ()); // Too short
        Assert.That(!"123456789012345".IsCPForCNPJ()); // Too long
        Assert.That(!"".IsCPForCNPJ()); // Empty string
    }
}