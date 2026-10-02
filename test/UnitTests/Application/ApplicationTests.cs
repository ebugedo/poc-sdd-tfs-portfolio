using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

public class MappingTests
{
    private readonly IMapper _mapper;

    public MappingTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TestMappingProfile>();
        });
        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Mapping_EntityToDto_Works()
    {
        var entity = new TestEntity { Id = 1, Name = "Test" };
        var dto = _mapper.Map<TestDto>(entity);

        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal(entity.Name, dto.Name);
    }

    [Fact]
    public void Mapping_DtoToEntity_Works()
    {
        var dto = new TestDto { Id = 2, Name = "Test 2" };
        var entity = _mapper.Map<TestEntity>(dto);

        Assert.Equal(dto.Id, entity.Id);
        Assert.Equal(dto.Name, entity.Name);
    }
}

public class ValidationTests
{
    private readonly TestCommandValidator _validator = new();

    [Fact]
    public void ValidCommand_PassesValidation()
    {
        var command = new TestCommand { Name = "Valid Name" };
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
}

public class HandlerPipelineTests
{
    [Fact]
    public async Task CommandHandler_ExecutesDirectly()
    {
        var handler = new TestCommandHandler();
        var command = new TestCommand { Name = "Test Command" };
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Command handled: Test Command", result);
    }

    [Fact]
    public async Task QueryHandler_ExecutesDirectly()
    {
        var handler = new TestQueryHandler();
        var query = new TestQuery { Id = 42 };
        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal("Query handled: 42", result);
    }
}