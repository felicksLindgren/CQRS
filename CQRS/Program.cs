using Application.Queries;
using CQRS.Commands;
using CQRS.Commands.Interfaces;
using CQRS.Queries;
using CQRS.Queries.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Dependency Injection

builder.Services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
builder.Services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();

// INFO: Using https://www.nuget.org/packages/Scrutor for registering all Query and Command handlers by convention
builder.Services.Scan(selector =>
{
    selector.FromApplicationDependencies()
            .AddClasses(filter =>
            {
                filter.AssignableTo(typeof(IQueryHandler<,>));
            })
            .AsImplementedInterfaces()
            .WithSingletonLifetime();
});
builder.Services.Scan(selector =>
{
    selector.FromApplicationDependencies()
            .AddClasses(filter =>
            {
                filter.AssignableTo(typeof(ICommandHandler<,>));
            })
            .AsImplementedInterfaces()
            .WithSingletonLifetime();
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/weatherforecast", async (IQueryDispatcher dispatcher, CancellationToken cancellationToken) =>
{
    var query = new WeatherForecastQuery();

    var forecast = await dispatcher.Dispatch<WeatherForecastQuery, IEnumerable<WeatherForecast>>(query, cancellationToken);

    return forecast;
});

app.Run();