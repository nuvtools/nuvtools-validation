using NUnit.Framework;

namespace NuvTools.Validation.Tests;

[TestFixture()]
public class EmailValidatorExtensions
{
    [TestCase("contato@credora.com.br")]
    [TestCase("Financeiro.Credora@Empresa.COM")]
    [TestCase("contato+cobranca@credora.com.br")]
    [TestCase("contato_financeiro@sub-dominio.com.br")]
    [TestCase("a!#$%&'*/=?^`{|}~-b@example.com")]
    [TestCase("\"john doe\"@example.com")]
    [TestCase("\"john\\\"doe\"@example.com")]
    [TestCase("user@[192.168.0.1]")]
    [TestCase("user@[IPv6:2001:db8::1]")]
    public void ValidateIsEmail(string email)
    {
        Assert.That(email.IsEmail());
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    [TestCase("email-invalido")]
    [TestCase("contato@gmai..com")]
    [TestCase("contato@gmail")]
    [TestCase("contato@.com")]
    [TestCase("contato@gmail.c")]
    [TestCase("contato@gmail.c0m")]
    [TestCase(".contato@gmail.com")]
    [TestCase("contato.@gmail.com")]
    [TestCase("contato..financeiro@gmail.com")]
    [TestCase("contato @gmail.com")]
    [TestCase("contato@gmail@com")]
    [TestCase("contato@-gmail.com")]
    [TestCase("contato@gmail-.com")]
    [TestCase("joão@gmail.com")]
    [TestCase("\"unterminated@example.com")]
    [TestCase("user@[999.168.0.1]")]
    [TestCase("user@[IPv6:not-an-ip]")]
    public void ValidateIsNotEmail(string? email)
    {
        Assert.That(!email.IsEmail());
    }

    [Test()]
    public void ValidateIsEmailLengthLimits()
    {
        Assert.That($"{new string('a', 64)}@example.com".IsEmail());
        Assert.That(!$"{new string('a', 65)}@example.com".IsEmail());
        Assert.That(!$"user@{new string('a', 64)}.com".IsEmail());
        Assert.That(!$"{new string('a', 64)}@{new string('b', 63)}.{new string('c', 63)}.{new string('d', 63)}.com".IsEmail());
    }
}
