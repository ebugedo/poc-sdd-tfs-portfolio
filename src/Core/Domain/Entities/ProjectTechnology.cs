using Portfolio.Domain;

namespace Portfolio.Domain.Entities;

public sealed class ProjectTechnology : Entity<Guid>
{
    public Guid ProjectId { get; private set; }
    public Guid TechnologyId { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public string? Notes { get; private set; }

    private ProjectTechnology() { }

    private ProjectTechnology(Guid id, Guid projectId, Guid technologyId, string? notes)
        : base(id)
    {
        ProjectId = projectId;
        TechnologyId = technologyId;
        AssignedAt = DateTime.UtcNow;
        Notes = notes?.Trim();
    }

    public static ProjectTechnology Create(Guid projectId, Guid technologyId, string? notes = null)
    {
        return new ProjectTechnology(Guid.NewGuid(), projectId, technologyId, notes);
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes?.Trim();
    }
}