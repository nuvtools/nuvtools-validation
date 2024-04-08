using NUnit.Framework;
using NuvTools.Validation.Brazil;

namespace NuvTools.Validation.Tests.Brazil;

[TestFixture()]
public class CNPJandCPFExtensions
{
    [Test()]
    public void ValidateCNPJ()
    {
        Assert.That(!"11111111111111".IsCNPJ());
        Assert.That("03.785.417/0001-03".IsCNPJ());
        Assert.That("54243121000193".IsCNPJ());
    }

    [Test()]
    public void ValidateCPF()
    {
        Assert.That(!"11111111111".IsCPF());
        Assert.That(!"22222222222".IsCPF());
        Assert.That(!"erro".IsCPF());
        Assert.That("583.008.930-08".IsCPF());
        Assert.That("83289988074".IsCPF());
    }

}