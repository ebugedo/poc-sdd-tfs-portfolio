using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Application;
using Portfolio.Infrastructure.Persistence;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// AutoMapper via Microsoft DI extension
builder.Services.AddAutoMapper(typeof(ApplicationModule).Assembly);

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new ApplicationModule());
    containerBuilder.RegisterModule(new PersistenceModule());
    containerBuilder.RegisterModule(new Portfolio.Presentation.WebAPI.WebApiModule());
});

// ProblemDetails (Hellang)
Hellang.Middleware.ProblemDetails.ProblemDetailsExtensions.AddProblemDetails(builder.Services);

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "default-secret-key-change-in-production"))
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:3000" })
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Controllers/Minimal APIs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseProblemDetails();
app.UseHttpsRedirection();
app.UseCors("Default");
app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy" }))
   .WithName("HealthCheck")
   .WithOpenApi()
   .AllowAnonymous();

// API versioning with MapGroup
var apiV1 = app.MapGroup("/api/v1")
    .WithOpenApi()
    .RequireAuthorization();

// Placeholder for future endpoints
apiV1.MapGet("/test", () => Results.Ok(new { Message = "API v1 is working" }))
     .WithName("TestEndpoint")
     .AllowAnonymous();

app.Run();