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
        Assert.That("3785417000133".FormatCNPJ(), Is.EqualTo("03.785.417/0001-33"));
        Assert.That("54243121000193".FormatCNPJ(), Is.EqualTo("54.243.121/0001-93"));
        Assert.That("00000000000000".FormatCNPJ(), Is.EqualTo("00.000.000/0000-00"));
    }

    [Test]
    public void FormatCNPJ_Invalid()
    {
        Assert.Throws<FormatException>(() => "".FormatCNPJ());
        Assert.Throws<FormatException>(() => "abc".FormatCNPJ());
        Assert.Throws<OverflowException>(() => "999999999999999999999".FormatCNPJ());
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