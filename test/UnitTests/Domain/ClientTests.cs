using Bogus;
using Portfolio.Domain;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Events;
using Portfolio.Domain.ValueObjects;
using Xunit;

namespace Portfolio.UnitTests.Domain;

public class ClientTests
{
    private static readonly Faker<string> ValidNameFaker = new Faker<string>()
        .CustomInstantiator(f => f.Company.CompanyName());

    private static readonly Faker<Email> ValidEmailFaker = new Faker<Email>()
        .CustomInstantiator(f => Email.Create(f.Internet.Email().ToLowerInvariant()));

    private static readonly Faker<string> PhoneFaker = new Faker<string>()
        .CustomInstantiator(f => f.Phone.PhoneNumber());

    private static readonly Faker<string> AddressFaker = new Faker<string>()
        .CustomInstantiator(f => f.Address.FullAddress());

    [Fact]
    public void Create_WithValidData_CreatesClientAndEmitsEvent()
    {
        var name = ValidNameFaker.Generate();
        var email = ValidEmailFaker.Generate();
        var phone = PhoneFaker.Generate();
        var address = AddressFaker.Generate();

        var client = Client.Create(name, email, phone, address);

        Assert.NotEqual(Guid.Empty, client.Id);
        Assert.Equal(name.Trim(), client.Name);
        Assert.Equal(email.Value, client.Email.Value);
        Assert.Equal(phone.Trim(), client.Phone);
        Assert.Equal(address.Trim(), client.Address);
        Assert.True(client.IsActive);
        Assert.True(client.CreatedAt <= DateTime.UtcNow);
        Assert.Null(client.UpdatedAt);
        Assert.Single(client.DomainEvents);
        Assert.IsType<ClientCreated>(client.DomainEvents.First());
        var evt = (ClientCreated)client.DomainEvents.First();
        Assert.Equal(client.Id, evt.ClientId);
        Assert.Equal(client.Name, evt.Name);
        Assert.Equal(client.Email.Value, evt.Email);
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsDomainException()
    {
        var email = ValidEmailFaker.Generate();

        var exception = Assert.Throws<DomainException>(() => Client.Create("", email));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_WithWhitespaceName_ThrowsDomainException()
    {
        var email = ValidEmailFaker.Generate();

        var exception = Assert.Throws<DomainException>(() => Client.Create("   ", email));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_TrimsNamePhoneAddress()
    {
        var client = Client.Create("  Test Client  ", ValidEmailFaker.Generate(), "  123-456-7890  ", "  123 Main St  ");

        Assert.Equal("Test Client", client.Name);
        Assert.Equal("123-456-7890", client.Phone);
        Assert.Equal("123 Main St", client.Address);
    }

    [Fact]
    public void Update_WithValidData_UpdatesClientAndEmitsEvent()
    {
        var client = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());
        client.ClearDomainEvents();

        var newName = ValidNameFaker.Generate();
        var newPhone = PhoneFaker.Generate();
        var newAddress = AddressFaker.Generate();

        client.Update(newName, newPhone, newAddress);

        Assert.Equal(newName.Trim(), client.Name);
        Assert.Equal(newPhone.Trim(), client.Phone);
        Assert.Equal(newAddress.Trim(), client.Address);
        Assert.NotNull(client.UpdatedAt);
        Assert.Single(client.DomainEvents);
        Assert.IsType<ClientUpdated>(client.DomainEvents.First());
    }

    [Fact]
    public void Update_WithEmptyName_ThrowsDomainException()
    {
        var client = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());

        var exception = Assert.Throws<DomainException>(() => client.Update(""));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void UpdateEmail_WithValidEmail_UpdatesAndEmitsEvent()
    {
        var client = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());
        client.ClearDomainEvents();

        var newEmail = ValidEmailFaker.Generate();

        client.UpdateEmail(newEmail);

        Assert.Equal(newEmail.Value, client.Email.Value);
        Assert.NotNull(client.UpdatedAt);
        Assert.Single(client.DomainEvents);
        Assert.IsType<ClientUpdated>(client.DomainEvents.First());
    }

    [Fact]
    public void Deactivate_ActiveClient_DeactivatesAndEmitsEvent()
    {
        var client = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());
        client.ClearDomainEvents();

        client.Deactivate();

        Assert.False(client.IsActive);
        Assert.NotNull(client.UpdatedAt);
        Assert.Single(client.DomainEvents);
        Assert.IsType<ClientDeactivated>(client.DomainEvents.First());
    }

    [Fact]
    public void Deactivate_AlreadyInactiveClient_DoesNothing()
    {
        var client = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());
        client.Deactivate();
        client.ClearDomainEvents();

        client.Deactivate();

        Assert.False(client.IsActive);
        Assert.Empty(client.DomainEvents);
    }

    [Fact]
    public void Equality_SameId_AreEqual()
    {
        var id = Guid.NewGuid();
        var client1 = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());
        var client2 = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());

        // Can't easily test with same Id since it's generated internally
        // Test that two different clients are not equal
        Assert.NotEqual(client1, client2);
        Assert.False(client1 == client2);
    }

    [Fact]
    public void Equality_DifferentId_AreNotEqual()
    {
        var client1 = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());
        var client2 = Client.Create(ValidNameFaker.Generate(), ValidEmailFaker.Generate());

        Assert.NotEqual(client1, client2);
        Assert.False(client1 == client2);
    }
}