namespace Core.DTOs
{
    public record OpenWeatherMain
    {
        public decimal Temp { get; init; }
        public decimal Temp_Min { get; init; }
        public decimal Temp_Max { get; init; }
    }

    public record OpenWeatherResponse
    {
        public OpenWeatherMain Main { get; init; }
    }
}
