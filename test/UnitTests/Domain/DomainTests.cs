using Portfolio.Domain;
using Xunit;

namespace Portfolio.UnitTests.Domain;

public record TestId(Guid Value);

public class TestEntity : Entity<TestId>
{
    public string Name { get; private set; } = string.Empty;

    public TestEntity(TestId id, string name) : base(id)
    {
        Name = name;
    }
}

public class TestValueObject : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }

    public TestValueObject(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}

public class TestAggregateRoot : AggregateRoot<TestId>
{
    public string Name { get; private set; } = string.Empty;

    public TestAggregateRoot(TestId id, string name) : base(id)
    {
        Name = name;
    }

    public void AddTestEvent()
    {
        AddDomainEvent(new TestDomainEvent(Name));
    }
}

public class TestDomainEvent : DomainEvent
{
    public string AggregateName { get; }

    public TestDomainEvent(string aggregateName)
    {
        AggregateName = aggregateName;
    }
}

public class EntityTests
{
    [Fact]
    public void Entities_WithSameId_AreEqual()
    {
        var id = new TestId(Guid.NewGuid());
        var entity1 = new TestEntity(id, "Entity 1");
        var entity2 = new TestEntity(id, "Entity 2");

        Assert.Equal(entity1, entity2);
        Assert.True(entity1 == entity2);
    }

    [Fact]
    public void Entities_WithDifferentId_AreNotEqual()
    {
        var entity1 = new TestEntity(new TestId(Guid.NewGuid()), "Entity 1");
        var entity2 = new TestEntity(new TestId(Guid.NewGuid()), "Entity 2");

        Assert.NotEqual(entity1, entity2);
        Assert.False(entity1 == entity2);
    }

    [Fact]
    public void Entity_WithDefaultId_AreEqual()
    {
        var entity = new TestEntity(default, "Entity");
        var other = new TestEntity(default, "Other");

        Assert.Equal(entity, other);
    }
}

public class ValueObjectTests
{
    [Fact]
    public void ValueObjects_WithSameComponents_AreEqual()
    {
        var vo1 = new TestValueObject("John", "Doe");
        var vo2 = new TestValueObject("John", "Doe");

        Assert.Equal(vo1, vo2);
        Assert.True(vo1 == vo2);
        Assert.Equal(vo1.GetHashCode(), vo2.GetHashCode());
    }

    [Fact]
    public void ValueObjects_WithDifferentComponents_AreNotEqual()
    {
        var vo1 = new TestValueObject("John", "Doe");
        var vo2 = new TestValueObject("Jane", "Doe");

        Assert.NotEqual(vo1, vo2);
        Assert.False(vo1 == vo2);
    }

    [Fact]
    public void ValueObject_WithNullComponent_HandlesCorrectly()
    {
        var vo1 = new TestValueObject("John", null!);
        var vo2 = new TestValueObject("John", null!);

        Assert.Equal(vo1, vo2);
    }
}

public class AggregateRootTests
{
    [Fact]
    public void AggregateRoot_CanAddAndClearDomainEvents()
    {
        var aggregate = new TestAggregateRoot(new TestId(Guid.NewGuid()), "Test Aggregate");

        Assert.Empty(aggregate.DomainEvents);

        aggregate.AddTestEvent();

        Assert.Single(aggregate.DomainEvents);
        Assert.IsType<TestDomainEvent>(aggregate.DomainEvents.First());

        aggregate.ClearDomainEvents();

        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void AggregateRoot_DomainEventsAreReadOnly()
    {
        var aggregate = new TestAggregateRoot(new TestId(Guid.NewGuid()), "Test Aggregate");
        aggregate.AddTestEvent();

        var events = aggregate.DomainEvents;

        Assert.IsAssignableFrom<IReadOnlyCollection<IDomainEvent>>(events);
        Assert.Equal(1, events.Count);
    }
}

public class DomainExceptionTests
{
    [Fact]
    public void DomainException_HasDefaultCode()
    {
        var ex = new DomainException("Test error");

        Assert.Equal("DOMAIN_ERROR", ex.Code);
        Assert.Equal("Test error", ex.Message);
    }

    [Fact]
    public void DomainException_CanSetCustomCode()
    {
        var ex = new DomainException("Test error", "CUSTOM_CODE");

        Assert.Equal("CUSTOM_CODE", ex.Code);
    }

    [Fact]
    public void DomainException_CanWrapInnerException()
    {
        var inner = new InvalidOperationException("Inner error");
        var ex = new DomainException("Outer error", inner, "WRAPPED_ERROR");

        Assert.Equal("WRAPPED_ERROR", ex.Code);
        Assert.Equal(inner, ex.InnerException);
    }
}