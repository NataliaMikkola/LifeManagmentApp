
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

public class OpenMeteoWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public OpenMeteoWeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherInfo> GetCurrentWeatherAsync()
    {
        const string url =
            "https://api.open-meteo.com/v1/forecast" +
            "?latitude=63.10&longitude=21.62" +
            "&current=temperature_2m,wind_speed_10m" +
            "&wind_speed_unit=ms";

        var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(url);

        if (response?.Current == null)
            throw new Exception("Weather data is unavailable.");

        return new WeatherInfo
        {
            Temperature = response.Current.Temperature,
            WindSpeed = response.Current.WindSpeed
        };
    }

    private class WeatherResponse
    {
        [JsonPropertyName("current")]
        public CurrentWeather? Current { get; set; }
    }

    private class CurrentWeather
    {
        [JsonPropertyName("temperature_2m")]
        public double Temperature { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed { get; set; }
    }
}
