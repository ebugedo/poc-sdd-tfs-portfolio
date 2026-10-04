using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class TechnologyRemovedFromProject : DomainEvent
{
    public Guid ProjectId { get; }
    public Guid TechnologyId { get; }

    public TechnologyRemovedFromProject(Guid projectId, Guid technologyId)
    {
        ProjectId = projectId;
        TechnologyId = technologyId;
    }
}