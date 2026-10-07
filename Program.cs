using ToolTracker;
using FluentValidation;
using Scalar.AspNetCore;
using ToolTracker.Data;
using ToolTracker.Exceptions;
using ToolTracker.Models;
using ToolTracker.Models.DTOS;
using ToolTracker.Services;
using ToolTracker.Validation;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Exception handling
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Time provider
builder.Services.AddSingleton(TimeProvider.System);

// In-memory repositories
builder.Services.AddSingleton<IRepository<Member>, InMemoryRepository<Member>>();
builder.Services.AddSingleton<IRepository<Tool>, InMemoryRepository<Tool>>();
builder.Services.AddSingleton<ILoanRepository, LoanRepository>();

// Loan service
builder.Services.AddSingleton<LoanService>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateLoanRequestValidator>();

var app = builder.Build();

// Seed data
var members = app.Services.GetRequiredService<IRepository<Member>>();
var tools = app.Services.GetRequiredService<IRepository<Tool>>();

var member1 = new Member(
    Guid.Parse("11111111-1111-1111-1111-111111111111"),
    "Alice Smith",
    "alice@example.com"
);

var member2 = new Member(
    Guid.Parse("22222222-2222-2222-2222-222222222222"),
    "Bob Jones",
    "bob@example.com"
);

members.Add(member1);
members.Add(member2);

tools.Add(new Tool(
    Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
    "Cordless Drill",
    "Power Tools",
    member1.Id
));

tools.Add(new Tool(
    Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
    "Circular Saw",
    "Power Tools",
    member1.Id
));

// HTTP pipeline
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("ToolTracker API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;