using Portfolio.Domain.Events;
using Portfolio.Domain.ValueObjects;

namespace Portfolio.Domain.Entities;

public sealed class Client : AggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Client() { }

    private Client(Guid id, string name, Email email, string? phone, string? address)
        : base(id)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Client Create(string name, Email email, string? phone = null, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Client name is required", "NAME_REQUIRED");
        }

        var client = new Client(Guid.NewGuid(), name.Trim(), email, phone?.Trim(), address?.Trim());
        client.AddDomainEvent(new ClientCreated(client.Id, client.Name, client.Email.Value));
        return client;
    }

    public void Update(string name, string? phone = null, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Client name is required", "NAME_REQUIRED");
        }

        Name = name.Trim();
        Phone = phone?.Trim();
        Address = address?.Trim();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ClientUpdated(Id, Name, Email.Value, Phone, Address));
    }

    public void UpdateEmail(Email newEmail)
    {
        Email = newEmail;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ClientUpdated(Id, Name, Email.Value, Phone, Address));
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ClientDeactivated(Id));
    }
}