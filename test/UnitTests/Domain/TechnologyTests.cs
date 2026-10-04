using Portfolio.Domain;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Events;
using Xunit;

namespace Portfolio.UnitTests.Domain;

public class TechnologyTests
{
    [Fact]
    public void Create_WithValidData_CreatesTechnologyAndEmitsEvent()
    {
        var name = "React";
        var category = TechnologyCategory.Frontend;
        var description = "JavaScript library for building user interfaces";

        var technology = Technology.Create(name, category, description);

        Assert.NotEqual(Guid.Empty, technology.Id);
        Assert.Equal(name.Trim(), technology.Name);
        Assert.Equal(category, technology.Category);
        Assert.Equal(description.Trim(), technology.Description);
        Assert.True(technology.IsActive);
        Assert.True(technology.CreatedAt <= DateTime.UtcNow);
        Assert.Single(technology.DomainEvents);
        Assert.IsType<TechnologyCreated>(technology.DomainEvents.First());
        var evt = (TechnologyCreated)technology.DomainEvents.First();
        Assert.Equal(technology.Id, evt.TechnologyId);
        Assert.Equal(technology.Name, evt.Name);
        Assert.Equal(technology.Category, evt.Category);
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Technology.Create("", TechnologyCategory.Frontend));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_WithWhitespaceName_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Technology.Create("   ", TechnologyCategory.Frontend));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_WithInvalidCategory_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Technology.Create("Test", (TechnologyCategory)999));

        Assert.Equal("INVALID_TECHNOLOGY_CATEGORY", exception.Code);
    }

    [Fact]
    public void Create_TrimsNameAndDescription()
    {
        var technology = Technology.Create("  React  ", TechnologyCategory.Frontend, "  Description  ");

        Assert.Equal("React", technology.Name);
        Assert.Equal("Description", technology.Description);
    }

    [Fact]
    public void Create_WithoutDescription_SetsNull()
    {
        var technology = Technology.Create("React", TechnologyCategory.Frontend);

        Assert.Equal("React", technology.Name);
        Assert.Null(technology.Description);
    }

    [Fact]
    public void AllValidCategories_AreAccepted()
    {
        foreach (TechnologyCategory category in Enum.GetValues<TechnologyCategory>())
        {
            var technology = Technology.Create("Test", category);
            Assert.Equal(category, technology.Category);
        }
    }

    [Fact]
    public void Update_WithValidData_UpdatesTechnologyAndEmitsEvent()
    {
        var technology = Technology.Create("React", TechnologyCategory.Frontend, "Description");
        technology.ClearDomainEvents();

        var newName = "Vue";
        var newCategory = TechnologyCategory.Frontend;
        var newDescription = "New Description";

        technology.Update(newName, newCategory, newDescription);

        Assert.Equal(newName.Trim(), technology.Name);
        Assert.Equal(newCategory, technology.Category);
        Assert.Equal(newDescription.Trim(), technology.Description);
        Assert.Single(technology.DomainEvents);
        Assert.IsType<TechnologyUpdated>(technology.DomainEvents.First());
    }

    [Fact]
    public void Update_WithEmptyName_ThrowsDomainException()
    {
        var technology = Technology.Create("React", TechnologyCategory.Frontend);

        var exception = Assert.Throws<DomainException>(() => technology.Update("", TechnologyCategory.Frontend));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Update_WithInvalidCategory_ThrowsDomainException()
    {
        var technology = Technology.Create("React", TechnologyCategory.Frontend);

        var exception = Assert.Throws<DomainException>(() => technology.Update("New Name", (TechnologyCategory)999));

        Assert.Equal("INVALID_TECHNOLOGY_CATEGORY", exception.Code);
    }

    [Fact]
    public void Equality_DifferentId_AreNotEqual()
    {
        var tech1 = Technology.Create("React", TechnologyCategory.Frontend);
        var tech2 = Technology.Create("Vue", TechnologyCategory.Backend);

        Assert.NotEqual(tech1, tech2);
        Assert.False(tech1 == tech2);
    }
}