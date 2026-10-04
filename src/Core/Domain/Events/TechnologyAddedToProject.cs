using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class TechnologyAddedToProject : DomainEvent
{
    public Guid ProjectId { get; }
    public Guid TechnologyId { get; }

    public TechnologyAddedToProject(Guid projectId, Guid technologyId)
    {
        ProjectId = projectId;
        TechnologyId = technologyId;
    }
}