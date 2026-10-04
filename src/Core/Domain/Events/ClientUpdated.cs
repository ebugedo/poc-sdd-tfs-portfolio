using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class ClientUpdated : DomainEvent
{
    public Guid ClientId { get; }
    public string Name { get; }
    public string Email { get; }
    public string? Phone { get; }
    public string? Address { get; }

    public ClientUpdated(Guid clientId, string name, string email, string? phone, string? address)
    {
        ClientId = clientId;
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
    }
}