using CQRS.Queries.Interfaces;

namespace Application.Queries
{
    #region Records

    public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    public record WeatherForecastQuery();

    #endregion

    public class WeatherForecastQueryHandler : IQueryHandler<WeatherForecastQuery, IEnumerable<WeatherForecast>>
    {
        private readonly string[] _summaries;
        public WeatherForecastQueryHandler() => _summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        public async Task<IEnumerable<WeatherForecast>> Handle(WeatherForecastQuery query, CancellationToken cancellationToken)
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateTime.Now.AddDays(index),
                Random.Shared.Next(-20, 55),
                _summaries[Random.Shared.Next(_summaries.Length)]
            )).ToArray();

            var task = Task.Run(() => forecast);
            return await task;
        }
    }
}
