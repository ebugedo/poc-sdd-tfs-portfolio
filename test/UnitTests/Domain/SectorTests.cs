using Bogus;
using Portfolio.Domain;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Events;
using Xunit;

namespace Portfolio.UnitTests.Domain;

public class SectorTests
{
    private static readonly Faker<string> ValidNameFaker = new Faker<string>()
        .CustomInstantiator(f => f.Commerce.Categories(1)[0]);

    private static readonly Faker<string> DescriptionFaker = new Faker<string>()
        .CustomInstantiator(f => f.Lorem.Sentence());

    [Fact]
    public void Create_WithValidData_CreatesSectorAndEmitsEvent()
    {
        var name = ValidNameFaker.Generate();
        var description = DescriptionFaker.Generate();

        var sector = Sector.Create(name, description);

        Assert.NotEqual(Guid.Empty, sector.Id);
        Assert.Equal(name.Trim(), sector.Name);
        Assert.Equal(description.Trim(), sector.Description);
        Assert.True(sector.IsActive);
        Assert.True(sector.CreatedAt <= DateTime.UtcNow);
        Assert.Single(sector.DomainEvents);
        Assert.IsType<SectorCreated>(sector.DomainEvents.First());
        var evt = (SectorCreated)sector.DomainEvents.First();
        Assert.Equal(sector.Id, evt.SectorId);
        Assert.Equal(sector.Name, evt.Name);
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Sector.Create(""));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_WithWhitespaceName_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() => Sector.Create("   "));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Create_TrimsNameAndDescription()
    {
        var sector = Sector.Create("  Technology  ", "  Description  ");

        Assert.Equal("Technology", sector.Name);
        Assert.Equal("Description", sector.Description);
    }

    [Fact]
    public void Create_WithoutDescription_SetsNull()
    {
        var sector = Sector.Create("Technology");

        Assert.Equal("Technology", sector.Name);
        Assert.Null(sector.Description);
    }

    [Fact]
    public void Update_WithValidData_UpdatesSectorAndEmitsEvent()
    {
        var sector = Sector.Create(ValidNameFaker.Generate(), DescriptionFaker.Generate());
        sector.ClearDomainEvents();

        var newName = ValidNameFaker.Generate();
        var newDescription = DescriptionFaker.Generate();

        sector.Update(newName, newDescription);

        Assert.Equal(newName.Trim(), sector.Name);
        Assert.Equal(newDescription.Trim(), sector.Description);
        Assert.Single(sector.DomainEvents);
        Assert.IsType<SectorUpdated>(sector.DomainEvents.First());
    }

    [Fact]
    public void Update_WithEmptyName_ThrowsDomainException()
    {
        var sector = Sector.Create(ValidNameFaker.Generate());

        var exception = Assert.Throws<DomainException>(() => sector.Update(""));

        Assert.Equal("NAME_REQUIRED", exception.Code);
    }

    [Fact]
    public void Update_WithoutDescription_SetsNull()
    {
        var sector = Sector.Create("Technology", "Description");
        sector.ClearDomainEvents();

        sector.Update("New Technology", null);

        Assert.Equal("New Technology", sector.Name);
        Assert.Null(sector.Description);
    }

    [Fact]
    public void Equality_SameId_AreEqual()
    {
        var sector1 = Sector.Create(ValidNameFaker.Generate());
        var sector2 = Sector.Create(ValidNameFaker.Generate());

        Assert.NotEqual(sector1, sector2);
        Assert.False(sector1 == sector2);
    }

    [Fact]
    public void Equality_DifferentId_AreNotEqual()
    {
        var sector1 = Sector.Create(ValidNameFaker.Generate());
        var sector2 = Sector.Create(ValidNameFaker.Generate());

        Assert.NotEqual(sector1, sector2);
        Assert.False(sector1 == sector2);
    }
}