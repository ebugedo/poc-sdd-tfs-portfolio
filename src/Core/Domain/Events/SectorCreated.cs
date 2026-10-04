using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class SectorCreated : DomainEvent
{
    public Guid SectorId { get; }
    public string Name { get; }

    public SectorCreated(Guid sectorId, string name)
    {
        SectorId = sectorId;
        Name = name;
    }
}