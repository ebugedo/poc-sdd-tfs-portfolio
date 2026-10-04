using Bogus;
using Portfolio.Domain;
using Portfolio.Domain.ValueObjects;
using Xunit;

namespace Portfolio.UnitTests.Domain;

public class EmailTests
{
    private static readonly Faker<string> ValidEmailFaker = new Faker<string>()
        .CustomInstantiator(f => f.Internet.Email().ToLowerInvariant());

    private static readonly Faker<string> InvalidEmailFaker = new Faker<string>()
        .CustomInstantiator(f => f.Lorem.Word());

    [Fact]
    public void Create_WithValidEmail_ReturnsEmailInstance()
    {
        var email = ValidEmailFaker.Generate();

        var result = Email.Create(email);

        Assert.Equal(email.ToLowerInvariant(), result.Value);
    }

    [Fact]
    public void Create_WithValidEmailMixedCase_NormalizesToLowercase()
    {
        var email = "User@Example.COM";

        var result = Email.Create(email);

        Assert.Equal("user@example.com", result.Value);
    }

    [Fact]
    public void Create_WithValidEmailWithWhitespace_TrimsAndNormalizes()
    {
        var email = "  user@example.com  ";

        var result = Email.Create(email);

        Assert.Equal("user@example.com", result.Value);
    }

    [Fact]
    public void Create_WithEmptyString_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Email.Create(""));

        Assert.Equal("EMAIL_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_WithWhitespaceOnly_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Email.Create("   "));

        Assert.Equal("EMAIL_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_WithNull_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Email.Create(null!));

        Assert.Equal("EMAIL_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_WithInvalidFormat_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Email.Create("invalid-email"));

        Assert.Equal("INVALID_EMAIL_FORMAT", exception.Code);
    }

    [Fact]
    public void Create_WithMissingAtSymbol_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Email.Create("userexample.com"));

        Assert.Equal("INVALID_EMAIL_FORMAT", exception.Code);
    }

    [Fact]
    public void Create_WithMissingDomain_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Email.Create("user@"));

        Assert.Equal("INVALID_EMAIL_FORMAT", exception.Code);
    }

    [Fact]
    public void Create_WithMissingTld_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Email.Create("user@example"));

        Assert.Equal("INVALID_EMAIL_FORMAT", exception.Code);
    }

    [Fact]
    public void Equality_SameValue_AreEqual()
    {
        var email1 = Email.Create("user@example.com");
        var email2 = Email.Create("USER@EXAMPLE.COM");

        Assert.Equal(email1, email2);
        Assert.True(email1 == email2);
        Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
    }

    [Fact]
    public void Equality_DifferentValue_AreNotEqual()
    {
        var email1 = Email.Create("user1@example.com");
        var email2 = Email.Create("user2@example.com");

        Assert.NotEqual(email1, email2);
        Assert.False(email1 == email2);
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsValue()
    {
        var email = Email.Create("user@example.com");

        string value = email;

        Assert.Equal("user@example.com", value);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var email = Email.Create("user@example.com");

        Assert.Equal("user@example.com", email.ToString());
    }

    [Fact]
    public void Faker_GeneratesValidEmails()
    {
        var emailStr = ValidEmailFaker.Generate();
        var email = Email.Create(emailStr);

        Assert.False(string.IsNullOrWhiteSpace(email.Value));
        Assert.Contains("@", email.Value);
    }
}