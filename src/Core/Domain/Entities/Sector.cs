using Portfolio.Domain.Events;

namespace Portfolio.Domain.Entities;

public sealed class Sector : AggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Sector() { }

    private Sector(Guid id, string name, string? description)
        : base(id)
    {
        Name = name;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Sector Create(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Sector name is required", "NAME_REQUIRED");
        }

        var sector = new Sector(Guid.NewGuid(), name.Trim(), description?.Trim());
        sector.AddDomainEvent(new SectorCreated(sector.Id, sector.Name));
        return sector;
    }

    public void Update(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Sector name is required", "NAME_REQUIRED");
        }

        Name = name.Trim();
        Description = description?.Trim();

        AddDomainEvent(new SectorUpdated(Id, Name, Description));
    }
}