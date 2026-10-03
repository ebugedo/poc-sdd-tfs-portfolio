using AutoMapper;
using Bogus;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Portfolio.Application.Commands;
using Portfolio.Application.Mapping;
using Portfolio.Application.Queries;
using Xunit;

namespace Portfolio.UnitTests.Application;

public class TestCommand : CommandBase<string>
{
    public string Name { get; init; } = string.Empty;
}

public class TestQuery : QueryBase<string>
{
    public int Id { get; init; }
}

public class TestCommandValidator : AbstractValidator<TestCommand>
{
    public TestCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
    }
}

public class TestCommandHandler : IRequestHandler<TestCommand, string>
{
    public Task<string> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Command handled: {request.Name}");
    }
}

public class TestQueryHandler : IRequestHandler<TestQuery, string>
{
    public Task<string> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Query handled: {request.Id}");
    }
}

public class TestEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestMappingProfile : MappingProfile
{
    public TestMappingProfile()
    {
        CreateMap<TestEntity, TestDto>();
        CreateMap<TestDto, TestEntity>();
    }
}

// Bogus Fakers for Application layer test data
public static class ApplicationTestFakers
{
    public static readonly Faker<TestCommand> TestCommandFaker = new Faker<TestCommand>()
        .RuleFor(c => c.Name, f => f.Lorem.Sentence(3));

    public static readonly Faker<TestQuery> TestQueryFaker = new Faker<TestQuery>()
        .RuleFor(q => q.Id, f => f.Random.Int(1, 1000));

    public static readonly Faker<TestEntity> TestEntityFaker = new Faker<TestEntity>()
        .RuleFor(e => e.Id, f => f.Random.Int(1, 10000))
        .RuleFor(e => e.Name, f => f.Commerce.ProductName());

    public static readonly Faker<TestDto> TestDtoFaker = new Faker<TestDto>()
        .RuleFor(d => d.Id, f => f.Random.Int(1, 10000))
        .RuleFor(d => d.Name, f => f.Commerce.ProductName());
}

public class MappingTests
{
    private readonly IMapper _mapper;

    public MappingTests()
    {
        using var loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder => builder.AddConsole());
        var expression = new AutoMapper.MapperConfigurationExpression();
        expression.AddProfile<TestMappingProfile>();
        var config = new AutoMapper.MapperConfiguration(expression, loggerFactory);
        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Mapping_EntityToDto_Works()
    {
        var entity = ApplicationTestFakers.TestEntityFaker.Generate();
        var dto = _mapper.Map<TestDto>(entity);

        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal(entity.Name, dto.Name);
    }

    [Fact]
    public void Mapping_DtoToEntity_Works()
    {
        var dto = ApplicationTestFakers.TestDtoFaker.Generate();
        var entity = _mapper.Map<TestEntity>(dto);

        Assert.Equal(dto.Id, entity.Id);
        Assert.Equal(dto.Name, entity.Name);
    }

    [Fact]
    public void Mapping_Faker_GeneratesValidMappings()
    {
        var entities = ApplicationTestFakers.TestEntityFaker.Generate(10);
        var dtos = _mapper.Map<List<TestDto>>(entities);

        Assert.Equal(entities.Count, dtos.Count);
        for (int i = 0; i < entities.Count; i++)
        {
            Assert.Equal(entities[i].Id, dtos[i].Id);
            Assert.Equal(entities[i].Name, dtos[i].Name);
        }
    }

    [Fact]
    public void Mapping_Bidirectional_RoundTrip()
    {
        var entity = ApplicationTestFakers.TestEntityFaker.Generate();
        var dto = _mapper.Map<TestDto>(entity);
        var entity2 = _mapper.Map<TestEntity>(dto);

        Assert.Equal(entity.Id, entity2.Id);
        Assert.Equal(entity.Name, entity2.Name);
    }
}

public class ValidationTests
{
    private readonly TestCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_PassesValidation()
    {
        var command = ApplicationTestFakers.TestCommandFaker.Generate();
        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void InvalidCommand_EmptyName_FailsValidation()
    {
        var command = new TestCommand { Name = "" };
        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void InvalidCommand_ShortName_FailsValidation()
    {
        var command = new TestCommand { Name = "Ab" };
        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validator_Faker_GeneratesValidCommands()
    {
        var commands = ApplicationTestFakers.TestCommandFaker.Generate(50);
        foreach (var command in commands)
        {
            var result = _validator.Validate(command);
            Assert.True(result.IsValid, $"Command '{command.Name}' should be valid");
        }
    }

    [Fact]
    public void Validator_Faker_GeneratesDiverseNames()
    {
        var commands = ApplicationTestFakers.TestCommandFaker.Generate(100);
        var uniqueNames = commands.Select(c => c.Name).Distinct().Count();

        Assert.True(uniqueNames > 50);
    }
}

public class HandlerPipelineTests
{
    [Fact]
    public async Task CommandHandler_ExecutesDirectly()
    {
        var handler = new TestCommandHandler();
        var command = ApplicationTestFakers.TestCommandFaker.Generate();
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal($"Command handled: {command.Name}", result);
    }

    [Fact]
    public async Task QueryHandler_ExecutesDirectly()
    {
        var handler = new TestQueryHandler();
        var query = ApplicationTestFakers.TestQueryFaker.Generate();
        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal($"Query handled: {query.Id}", result);
    }

    [Fact]
    public async Task HandlerPipeline_Faker_GeneratesValidCommands()
    {
        var handler = new TestCommandHandler();
        var commands = ApplicationTestFakers.TestCommandFaker.Generate(10);

        foreach (var command in commands)
        {
            var result = await handler.Handle(command, CancellationToken.None);
            Assert.Equal($"Command handled: {command.Name}", result);
        }
    }
}