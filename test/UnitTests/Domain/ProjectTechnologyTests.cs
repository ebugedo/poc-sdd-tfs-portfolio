using Portfolio.Domain;
using Portfolio.Domain.Entities;
using Xunit;

namespace Portfolio.UnitTests.Domain;

public class ProjectTechnologyTests
{
    [Fact]
    public void Create_WithValidData_CreatesProjectTechnology()
    {
        var projectId = Guid.NewGuid();
        var technologyId = Guid.NewGuid();
        var notes = "Frontend framework";

        var projectTechnology = ProjectTechnology.Create(projectId, technologyId, notes);

        Assert.NotEqual(Guid.Empty, projectTechnology.Id);
        Assert.Equal(projectId, projectTechnology.ProjectId);
        Assert.Equal(technologyId, projectTechnology.TechnologyId);
        Assert.Equal(notes, projectTechnology.Notes);
        Assert.True(projectTechnology.AssignedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Create_WithoutNotes_SetsNull()
    {
        var projectTechnology = ProjectTechnology.Create(Guid.NewGuid(), Guid.NewGuid());

        Assert.Null(projectTechnology.Notes);
    }

    [Fact]
    public void UpdateNotes_UpdatesNotes()
    {
        var projectTechnology = ProjectTechnology.Create(Guid.NewGuid(), Guid.NewGuid(), "Old notes");

        projectTechnology.UpdateNotes("New notes");

        Assert.Equal("New notes", projectTechnology.Notes);
    }

    [Fact]
    public void UpdateNotes_ToNull_SetsNull()
    {
        var projectTechnology = ProjectTechnology.Create(Guid.NewGuid(), Guid.NewGuid(), "Old notes");

        projectTechnology.UpdateNotes(null);

        Assert.Null(projectTechnology.Notes);
    }

    [Fact]
    public void Equality_DifferentProjectOrTechnology_AreNotEqual()
    {
        var pt1 = ProjectTechnology.Create(Guid.NewGuid(), Guid.NewGuid());
        var pt2 = ProjectTechnology.Create(Guid.NewGuid(), Guid.NewGuid());

        Assert.NotEqual(pt1, pt2);
        Assert.False(pt1 == pt2);
    }
}