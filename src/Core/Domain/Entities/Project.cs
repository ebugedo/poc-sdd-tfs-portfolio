using System.Collections.Generic;
using Portfolio.Domain.Events;
using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Entities;

public sealed class Project : AggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid SectorId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public ProjectStatus Status { get; private set; }
    public decimal? Budget { get; private set; }
    public string Currency { get; private set; } = "USD";
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly List<ProjectTechnology> _technologies = new();
    public IReadOnlyCollection<ProjectTechnology> Technologies => _technologies.AsReadOnly();

    private Project() { }

    private Project(Guid id, string name, Guid clientId, Guid sectorId, DateTime startDate, string? description, DateTime? endDate, decimal? budget, string currency)
        : base(id)
    {
        Name = name;
        ClientId = clientId;
        SectorId = sectorId;
        StartDate = startDate;
        Description = description;
        EndDate = endDate;
        Status = ProjectStatus.Planning;
        Budget = budget;
        Currency = currency;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Project Create(string name, Guid clientId, Guid sectorId, DateTime startDate, string? description = null, DateTime? endDate = null, decimal? budget = null, string currency = "USD")
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Project name is required", "NAME_REQUIRED");
        }

        if (endDate.HasValue && endDate.Value < startDate)
        {
            throw new DomainException("End date cannot be before start date", "END_DATE_BEFORE_START_DATE");
        }

        var project = new Project(Guid.NewGuid(), name.Trim(), clientId, sectorId, startDate, description?.Trim(), endDate, budget, currency);
        project.AddDomainEvent(new ProjectCreated(project.Id, project.Name, project.ClientId, project.SectorId));
        return project;
    }

    public void Update(string name, string? description = null, DateTime? endDate = null, decimal? budget = null, string? currency = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Project name is required", "NAME_REQUIRED");
        }

        if (endDate.HasValue && endDate.Value < StartDate)
        {
            throw new DomainException("End date cannot be before start date", "END_DATE_BEFORE_START_DATE");
        }

        var oldName = Name;
        Name = name.Trim();
        Description = description?.Trim();
        EndDate = endDate;
        Budget = budget;
        Currency = currency ?? Currency;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ProjectUpdated(Id, Name, Description, ClientId, SectorId, StartDate, EndDate, Budget, Currency));
    }

    public void ChangeStatus(ProjectStatus newStatus)
    {
        var validTransitions = new Dictionary<ProjectStatus, HashSet<ProjectStatus>>
        {
            [ProjectStatus.Planning] = new() { ProjectStatus.InProgress, ProjectStatus.Cancelled, ProjectStatus.OnHold },
            [ProjectStatus.InProgress] = new() { ProjectStatus.Completed, ProjectStatus.Cancelled, ProjectStatus.OnHold },
            [ProjectStatus.OnHold] = new() { ProjectStatus.InProgress, ProjectStatus.Cancelled },
            [ProjectStatus.Completed] = new() { ProjectStatus.OnHold },
            [ProjectStatus.Cancelled] = new() { ProjectStatus.OnHold }
        };

        if (!validTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(newStatus))
        {
            throw new DomainException($"Invalid status transition from {Status} to {newStatus}", "INVALID_STATUS_TRANSITION");
        }

        var oldStatus = Status;
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ProjectStatusChanged(Id, oldStatus, newStatus));
    }

    public void AddTechnology(Guid technologyId, string? notes = null)
    {
        if (_technologies.Any(t => t.TechnologyId == technologyId))
        {
            throw new DomainException("Technology is already assigned to this project", "TECHNOLOGY_ALREADY_ASSIGNED");
        }

        var projectTechnology = ProjectTechnology.Create(Id, technologyId, notes);
        _technologies.Add(projectTechnology);

        AddDomainEvent(new TechnologyAddedToProject(Id, technologyId));
    }

    public void RemoveTechnology(Guid technologyId)
    {
        var projectTechnology = _technologies.FirstOrDefault(t => t.TechnologyId == technologyId);
        if (projectTechnology == null)
        {
            throw new DomainException("Technology is not assigned to this project", "TECHNOLOGY_NOT_ASSIGNED");
        }

        _technologies.Remove(projectTechnology);
        AddDomainEvent(new TechnologyRemovedFromProject(Id, technologyId));
    }
}