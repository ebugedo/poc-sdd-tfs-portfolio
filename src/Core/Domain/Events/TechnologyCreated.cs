using Portfolio.Domain;
using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Events;

public sealed class TechnologyCreated : DomainEvent
{
    public Guid TechnologyId { get; }
    public string Name { get; }
    public TechnologyCategory Category { get; }

    public TechnologyCreated(Guid technologyId, string name, TechnologyCategory category)
    {
        TechnologyId = technologyId;
        Name = name;
        Category = category;
    }
}