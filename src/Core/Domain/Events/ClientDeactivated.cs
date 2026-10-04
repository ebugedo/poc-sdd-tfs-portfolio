using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class ClientDeactivated : DomainEvent
{
    public Guid ClientId { get; }

    public ClientDeactivated(Guid clientId)
    {
        ClientId = clientId;
    }
}