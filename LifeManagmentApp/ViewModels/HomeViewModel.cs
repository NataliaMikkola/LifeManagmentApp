
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;
using LifeManagementApp.Services;

namespace LifeManagementApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly TodoService _todoService;
    private readonly IWeatherService _weatherService;

    public ObservableCollection<TodoItem> TopTasks { get; } = new();

    [ObservableProperty]
    private string temperatureText = "-- °C";

    [ObservableProperty]
    private string windSpeedText = "-- m/s";

    [ObservableProperty]
    private bool isKitesurfWeather;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public HomeViewModel(
        TodoService todoService,
        IWeatherService weatherService)
    {
        _todoService = todoService;
        _weatherService = weatherService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsLoading)
            return;

        IsLoading = true;
        ErrorMessage = string.Empty;
        IsKitesurfWeather = false;

        try
        {
            var tasks = await _todoService.GetTodosAsync();

            var topThree = tasks
                .Where(t => !t.IsDone)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .Take(3);

            TopTasks.Clear();

            foreach (var task in topThree)
                TopTasks.Add(task);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Tasks: {ex.Message}";
        }

        try
        {
            var weather = await _weatherService.GetCurrentWeatherAsync();

            TemperatureText = $"{weather.Temperature:F1} °C";
            WindSpeedText = $"{weather.WindSpeed:F1} m/s";
            IsKitesurfWeather = weather.IsKitesurfWeather;
        }
        catch (Exception)
        {
            TemperatureText = "-- °C";
            WindSpeedText = "-- m/s";
            IsKitesurfWeather = false;
            ErrorMessage += " Could not load weather. Check internet connection.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
