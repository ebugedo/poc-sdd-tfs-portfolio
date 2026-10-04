using Portfolio.Domain;
using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Events;

public sealed class ProjectStatusChanged : DomainEvent
{
    public Guid ProjectId { get; }
    public ProjectStatus OldStatus { get; }
    public ProjectStatus NewStatus { get; }

    public ProjectStatusChanged(Guid projectId, ProjectStatus oldStatus, ProjectStatus newStatus)
    {
        ProjectId = projectId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}