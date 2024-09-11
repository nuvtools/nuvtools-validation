using NUnit.Framework;
using System.Globalization;
using System.Text.RegularExpressions;

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


    [Test()]
    public void ValidateDataURIBase64()
    {
        string dataURIBase64 = "data:text/plain;base64,TnV2IFRvb2xz";

        Regex regex = new(RegexPattern.BASE64_DATAURI);
        var result = regex.Match(dataURIBase64);

        Assert.That(result.Groups["type"].Value == "text/plain");
        Assert.That(result.Groups["extension"].Value == "plain");
        Assert.That(result.Groups["content"].Value == "TnV2IFRvb2xz");
    }

    [Test()]
    public void ValidateBase64()
    {
        string base64Content = "TnV2IFRvb2xz";

        Regex regex = new(RegexPattern.BASE64_CONTENT);
        var result = regex.Match(base64Content);

        Assert.That(result.Groups["content"].Value == "TnV2IFRvb2xz");

        var restultB = regex.IsMatch(base64Content + "A");

        Assert.That(!restultB);
    }

}