using Portfolio.Domain;

namespace Portfolio.Domain.Events;

public sealed class ProjectUpdated : DomainEvent
{
    public Guid ProjectId { get; }
    public string Name { get; }
    public string? Description { get; }
    public Guid ClientId { get; }
    public Guid SectorId { get; }
    public DateTime StartDate { get; }
    public DateTime? EndDate { get; }
    public decimal? Budget { get; }
    public string Currency { get; }

    public ProjectUpdated(Guid projectId, string name, string? description, Guid clientId, Guid sectorId, DateTime startDate, DateTime? endDate, decimal? budget, string currency)
    {
        ProjectId = projectId;
        Name = name;
        Description = description;
        ClientId = clientId;
        SectorId = sectorId;
        StartDate = startDate;
        EndDate = endDate;
        Budget = budget;
        Currency = currency;
    }
}