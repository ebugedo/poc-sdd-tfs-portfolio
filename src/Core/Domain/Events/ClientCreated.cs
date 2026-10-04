using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class ClientCreated : DomainEvent
{
    public Guid ClientId { get; }
    public string Name { get; }
    public string Email { get; }

    public ClientCreated(Guid clientId, string name, string email)
    {
        ClientId = clientId;
        Name = name;
        Email = email;
    }
}