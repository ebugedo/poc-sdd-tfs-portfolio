using Portfolio.Domain;
using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Events;

public sealed class TechnologyUpdated : DomainEvent
{
    public Guid TechnologyId { get; }
    public string Name { get; }
    public string? Description { get; }
    public TechnologyCategory Category { get; }

    public TechnologyUpdated(Guid technologyId, string name, string? description, TechnologyCategory category)
    {
        TechnologyId = technologyId;
        Name = name;
        Description = description;
        Category = category;
    }
}