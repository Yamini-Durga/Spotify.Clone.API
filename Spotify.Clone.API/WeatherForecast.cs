namespace Spotify.Clone.API
{
    public class WeatherForecast
    {
        /// <summary>
        /// Current Date
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Temperature in C
        /// </summary>
        public int TemperatureC { get; set; }
        /// <summary>
        /// Temperature in F
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        /// <summary>
        /// Summary of weather
        /// </summary>
        public string? Summary { get; set; }
    }
}