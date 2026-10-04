using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class SectorUpdated : DomainEvent
{
    public Guid SectorId { get; }
    public string Name { get; }
    public string? Description { get; }

    public SectorUpdated(Guid sectorId, string name, string? description)
    {
        SectorId = sectorId;
        Name = name;
        Description = description;
    }
}