using NUnit.Framework;
using System.Globalization;

namespace NuvTools.Validation.Tests;

[TestFixture()]
public class ValidatorExtensions
{
    [Test()]
    public void ValidateHasNumbersOnly()
    {
        Assert.That("12345678910123456789101234567891012345678910123456789101234567891012345678910".HasNumbersOnly());
        Assert.That("12345678910".HasNumbersOnly());
        Assert.That(!"03.785.417".HasNumbersOnly());
        Assert.That(!"-54243121000193".HasNumbersOnly());
    }

    [Test()]
    public void ValidateIsIntNumber()
    {
        Assert.That("111111111".IsIntNumber());
        Assert.That(!"11111111111".IsIntNumber());
        Assert.That("-111111111".IsIntNumber());
        Assert.That(!"-111111111".IsIntNumber(true));
        Assert.That(!"erro".IsLongNumber());
        Assert.That(!"555.666.777-00".IsIntNumber());
    }

    [Test()]
    public void ValidateIsLongNumber()
    {
        Assert.That("11111111111".IsLongNumber());
        Assert.That("-11111111111".IsLongNumber());
        Assert.That(!"-11111111111".IsLongNumber(true));
        Assert.That(!"erro".IsLongNumber());
        Assert.That(!"555.666.777-00".IsLongNumber());
    }

    [Test()]
    public void ValidateIsDecimalNumber()
    {
        Assert.That($"583{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}08".IsDecimalNumber());
        Assert.That("11111111111".IsDecimalNumber());
        Assert.That("-11111111111".IsDecimalNumber());
        Assert.That(!"-11111111111".IsDecimalNumber(true));
        Assert.That(!"erro".IsDecimalNumber());
        Assert.That("83289988074".IsDecimalNumber());
    }

}