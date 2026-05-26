using NUnit.Framework;
using NuvTools.Validation.Brazil;
using System;

namespace NuvTools.Validation.Tests.Brazil;

[TestFixture]
public class BrazilFormatExtensions
{
    [Test]
    public void FormatCNPJ_Valid()
    {
        Assert.That("03785417000133".FormatCNPJ(), Is.EqualTo("03.785.417/0001-33"));
        Assert.That("54243121000193".FormatCNPJ(), Is.EqualTo("54.243.121/0001-93"));
        Assert.That("00000000000000".FormatCNPJ(), Is.EqualTo("00.000.000/0000-00"));

        // New RFB alphanumeric format
        Assert.That("12ABC34501DE35".FormatCNPJ(), Is.EqualTo("12.ABC.345/01DE-35"));
        Assert.That("12abc34501de35".FormatCNPJ(), Is.EqualTo("12.ABC.345/01DE-35"));
        Assert.That("12.ABC.345/01DE-35".FormatCNPJ(), Is.EqualTo("12.ABC.345/01DE-35"));
    }

    [Test]
    public void FormatCNPJ_Invalid()
    {
        Assert.Throws<FormatException>(() => "".FormatCNPJ());
        Assert.Throws<FormatException>(() => "abc".FormatCNPJ());
        Assert.Throws<FormatException>(() => "999999999999999999999".FormatCNPJ()); // Too long
        Assert.Throws<FormatException>(() => "3785417000133".FormatCNPJ()); // 13 chars (no auto-pad)
        Assert.Throws<ArgumentNullException>(() => ((string)null!).FormatCNPJ());
    }

    [Test]
    public void FormatCPF_Valid()
    {
        Assert.That("58300893008".FormatCPF(), Is.EqualTo("583.008.930-08"));
        Assert.That("83289988074".FormatCPF(), Is.EqualTo("832.899.880-74"));
        Assert.That("00000000000".FormatCPF(), Is.EqualTo("000.000.000-00"));
    }

    [Test]
    public void FormatCPF_Invalid()
    {
        Assert.Throws<FormatException>(() => "".FormatCPF());
        Assert.Throws<FormatException>(() => "abc".FormatCPF());
        Assert.Throws<OverflowException>(() => "999999999999999999999".FormatCPF());
    }
}