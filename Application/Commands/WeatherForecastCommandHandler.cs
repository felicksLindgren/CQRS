using Application.Entities;
using CQRS.Commands.Interfaces;

namespace Application.Commands
{
    public record AddWeatherForecastCommand(DateTime Date, int TemperatureC, string? Summary);

    public class WeatherForecastCommandHandler : ICommandHandler<AddWeatherForecastCommand, WeatherForecast>
    {
        public Task<WeatherForecast> Handle(AddWeatherForecastCommand query, CancellationToken cancellation)
        {
            var weatherForecast = new WeatherForecast(query.Date, query.TemperatureC, query.Summary);
            return Task.FromResult(weatherForecast);
        }
    }
}
