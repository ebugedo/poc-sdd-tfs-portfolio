using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class ProjectCreated : DomainEvent
{
    public Guid ProjectId { get; }
    public string Name { get; }
    public Guid ClientId { get; }
    public Guid SectorId { get; }

    public ProjectCreated(Guid projectId, string name, Guid clientId, Guid sectorId)
    {
        ProjectId = projectId;
        Name = name;
        ClientId = clientId;
        SectorId = sectorId;
    }
}