using Portfolio.Domain.Events;
using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public sealed class Technology : AggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TechnologyCategory Category { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Technology() { }

    private Technology(Guid id, string name, string? description, TechnologyCategory category)
        : base(id)
    {
        Name = name;
        Description = description;
        Category = category;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Technology Create(string name, TechnologyCategory category, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Technology name is required", "NAME_REQUIRED");
        }

        if (!Enum.IsDefined(typeof(TechnologyCategory), category))
        {
            throw new DomainException("Invalid technology category", "INVALID_TECHNOLOGY_CATEGORY");
        }

        var technology = new Technology(Guid.NewGuid(), name.Trim(), description?.Trim(), category);
        technology.AddDomainEvent(new TechnologyCreated(technology.Id, technology.Name, technology.Category));
        return technology;
    }

    public void Update(string name, TechnologyCategory category, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Technology name is required", "NAME_REQUIRED");
        }

        if (!Enum.IsDefined(typeof(TechnologyCategory), category))
        {
            throw new DomainException("Invalid technology category", "INVALID_TECHNOLOGY_CATEGORY");
        }

        Name = name.Trim();
        Category = category;
        Description = description?.Trim();

        AddDomainEvent(new TechnologyUpdated(Id, Name, Description, Category));
    }
}