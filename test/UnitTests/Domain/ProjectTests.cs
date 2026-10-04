using Portfolio.Domain;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Events;
using Xunit;

namespace Portfolio.UnitTests.Domain;

public class ProjectTests
{
    [Fact]
    public void Create_WithValidData_CreatesProjectAndEmitsEvent()
    {
        var name = "Portfolio Website";
        var clientId = Guid.NewGuid();
        var sectorId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-10);
        var description = "Company portfolio website";
        var endDate = DateTime.UtcNow.AddDays(30);
        var budget = 50000m;
        var currency = "USD";

        var project = Project.Create(name, clientId, sectorId, startDate, description, endDate, budget, currency);

        Assert.NotEqual(Guid.Empty, project.Id);
        Assert.Equal(name.Trim(), project.Name);
        Assert.Equal(clientId, project.ClientId);
        Assert.Equal(sectorId, project.SectorId);
        Assert.Equal(startDate, project.StartDate);
        Assert.Equal(description.Trim(), project.Description);
        Assert.Equal(endDate, project.EndDate);
        Assert.Equal(ProjectStatus.Planning, project.Status);
        Assert.Equal(budget, project.Budget);
        Assert.Equal(currency, project.Currency);
        Assert.True(project.CreatedAt <= DateTime.UtcNow);
        Assert.Null(project.UpdatedAt);
        Assert.Empty(project.Technologies);
        Assert.Single(project.DomainEvents);
        Assert.IsType<ProjectCreated>(project.DomainEvents.First());
        var evt = (ProjectCreated)project.DomainEvents.First();
        Assert.Equal(project.Id, evt.ProjectId);
        Assert.Equal(project.Name, evt.Name);
        Assert.Equal(project.ClientId, evt.ClientId);
        Assert.Equal(project.SectorId, evt.SectorId);
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Project.Create("", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_WithEndDateBeforeStartDate_ThrowsDomainException()
    {
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddDays(-1);

        var exception = Assert.Throws<DomainException>(() => Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), startDate, endDate: endDate));

        Assert.Equal("END_DATE_BEFORE_START_DATE", exception.Code);
    }

    [Fact]
    public void Create_WithSameStartAndEndDate_Succeeds()
    {
        var startDate = DateTime.UtcNow;

        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), startDate, endDate: startDate);

        Assert.Equal(startDate, project.EndDate);
    }

    [Fact]
    public void Create_Defaults_StatusPlanning_CurrencyUSD()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        Assert.Equal(ProjectStatus.Planning, project.Status);
        Assert.Equal("USD", project.Currency);
        Assert.Null(project.Budget);
        Assert.Null(project.EndDate);
        Assert.Null(project.Description);
    }

    [Fact]
    public void Update_WithValidData_UpdatesProjectAndEmitsEvent()
    {
        var project = Project.Create("Portfolio Website", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        project.ClearDomainEvents();

        var newName = "Updated Portfolio";
        var newDescription = "Updated description";
        var newEndDate = DateTime.UtcNow.AddDays(60);
        var newBudget = 75000m;
        var newCurrency = "EUR";

        project.Update(newName, newDescription, newEndDate, newBudget, newCurrency);

        Assert.Equal(newName.Trim(), project.Name);
        Assert.Equal(newDescription.Trim(), project.Description);
        Assert.Equal(newEndDate, project.EndDate);
        Assert.Equal(newBudget, project.Budget);
        Assert.Equal(newCurrency, project.Currency);
        Assert.NotNull(project.UpdatedAt);
        Assert.Single(project.DomainEvents);
        Assert.IsType<ProjectUpdated>(project.DomainEvents.First());
    }

    [Fact]
    public void Update_WithEmptyName_ThrowsDomainException()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        var exception = Assert.Throws<DomainException>(() => project.Update(""));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Update_WithEndDateBeforeStartDate_ThrowsDomainException()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(10));

        var exception = Assert.Throws<DomainException>(() => project.Update("New Name", endDate: DateTime.UtcNow));

        Assert.Equal("END_DATE_BEFORE_START_DATE", exception.Code);
    }

    [Fact]
    public void ChangeStatus_ValidTransitions_SucceedAndEmitEvent()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        // Planning -> InProgress
        project.ClearDomainEvents();
        project.ChangeStatus(ProjectStatus.InProgress);
        Assert.Equal(ProjectStatus.InProgress, project.Status);
        Assert.Single(project.DomainEvents);
        Assert.IsType<ProjectStatusChanged>(project.DomainEvents.First());

        // InProgress -> Completed
        project.ClearDomainEvents();
        project.ChangeStatus(ProjectStatus.Completed);
        Assert.Equal(ProjectStatus.Completed, project.Status);
        Assert.Single(project.DomainEvents);
        Assert.IsType<ProjectStatusChanged>(project.DomainEvents.First());

        // Completed -> OnHold
        project.ClearDomainEvents();
        project.ChangeStatus(ProjectStatus.OnHold);
        Assert.Equal(ProjectStatus.OnHold, project.Status);
        Assert.Single(project.DomainEvents);
        Assert.IsType<ProjectStatusChanged>(project.DomainEvents.First());

        // OnHold -> InProgress
        project.ClearDomainEvents();
        project.ChangeStatus(ProjectStatus.InProgress);
        Assert.Equal(ProjectStatus.InProgress, project.Status);
        Assert.Single(project.DomainEvents);
        Assert.IsType<ProjectStatusChanged>(project.DomainEvents.First());
    }

    [Fact]
    public void ChangeStatus_InvalidTransitions_ThrowDomainException()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        // Completed -> Planning (invalid)
        project.ChangeStatus(ProjectStatus.InProgress);
        project.ChangeStatus(ProjectStatus.Completed);
        var exception1 = Assert.Throws<DomainException>(() => project.ChangeStatus(ProjectStatus.Planning));
        Assert.Equal("INVALID_STATUS_TRANSITION", exception1.Code);

        // Cancelled -> InProgress (invalid)
        var project2 = Project.Create("Test2", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        project2.ChangeStatus(ProjectStatus.InProgress);
        project2.ChangeStatus(ProjectStatus.Cancelled);
        var exception2 = Assert.Throws<DomainException>(() => project2.ChangeStatus(ProjectStatus.InProgress));
        Assert.Equal("INVALID_STATUS_TRANSITION", exception2.Code);

        // Planning -> Completed (invalid - must go through InProgress)
        var project3 = Project.Create("Test3", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        var exception3 = Assert.Throws<DomainException>(() => project3.ChangeStatus(ProjectStatus.Completed));
        Assert.Equal("INVALID_STATUS_TRANSITION", exception3.Code);
    }

    [Fact]
    public void AddTechnology_WithValidTechnology_AddsAndEmitsEvent()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        var technologyId = Guid.NewGuid();
        project.ClearDomainEvents();

        project.AddTechnology(technologyId, "Frontend framework");

        Assert.Single(project.Technologies);
        Assert.Equal(project.Id, project.Technologies.First().ProjectId);
        Assert.Equal(technologyId, project.Technologies.First().TechnologyId);
        Assert.Equal("Frontend framework", project.Technologies.First().Notes);
        Assert.Single(project.DomainEvents);
        Assert.IsType<TechnologyAddedToProject>(project.DomainEvents.First());
    }

    [Fact]
    public void AddTechnology_Duplicate_ThrowsDomainException()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        var technologyId = Guid.NewGuid();

        project.AddTechnology(technologyId);

        var exception = Assert.Throws<DomainException>(() => project.AddTechnology(technologyId));

        Assert.Equal("TECHNOLOGY_ALREADY_ASSIGNED", exception.Code);
    }

    [Fact]
    public void RemoveTechnology_Existing_RemovesAndEmitsEvent()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        var technologyId = Guid.NewGuid();
        project.AddTechnology(technologyId);
        project.ClearDomainEvents();

        project.RemoveTechnology(technologyId);

        Assert.Empty(project.Technologies);
        Assert.Single(project.DomainEvents);
        Assert.IsType<TechnologyRemovedFromProject>(project.DomainEvents.First());
    }

    [Fact]
    public void RemoveTechnology_NonExistent_ThrowsDomainException()
    {
        var project = Project.Create("Test", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        var exception = Assert.Throws<DomainException>(() => project.RemoveTechnology(Guid.NewGuid()));

        Assert.Equal("TECHNOLOGY_NOT_ASSIGNED", exception.Code);
    }

    [Fact]
    public void Equality_DifferentId_AreNotEqual()
    {
        var project1 = Project.Create("Project1", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        var project2 = Project.Create("Project2", Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        Assert.NotEqual(project1, project2);
        Assert.False(project1 == project2);
    }
}