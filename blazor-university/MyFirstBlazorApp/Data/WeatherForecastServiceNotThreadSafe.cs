namespace MyFirstBlazorApp.Data;

public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class WeatherForecastServiceNotThreadSafe
{
    private volatile int Locked;

    private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    public async Task<WeatherForecast[]> GetForecastAsync(DateOnly startDate)
    {
        if (Interlocked.CompareExchange(ref Locked, 1, 0) > 0)
            throw new InvalidOperationException(
              "A second operation started on this context before a previous operation completed. Any "
              + "instance members are not guaranteed to be thread-safe.");

        try
        {
            await Task.Delay(3000);
            var rng = new Random();
            return [.. Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })];
        }
        finally
        {
            Interlocked.Decrement(ref Locked);
        }
    }
}