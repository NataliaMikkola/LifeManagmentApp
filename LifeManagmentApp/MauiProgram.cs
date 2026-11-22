
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using LifeManagementApp.Data;
using LifeManagementApp.Services;
using LifeManagementApp.ViewModels;
using LifeManagementApp.Views;

namespace LifeManagementApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont(
                        "OpenSans-Regular.ttf",
                        "OpenSansRegular");

                    fonts.AddFont(
                        "OpenSans-Semibold.ttf",
                        "OpenSansSemibold");
                });

            // SQLite database location.
            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "lifemanagement.db");

            // Register EF Core database factory.
            builder.Services.AddDbContextFactory<AppDbContext>(
                options => options.UseSqlite(
                    $"Data Source={dbPath}"));

            // Register database initialization service.
            builder.Services.AddSingleton<
                DatabaseInitializationService>();

            // Register repository interface and implementation.
            builder.Services.AddTransient<
                ITodoRepository, TodoService>();

            // Register Shell navigation.
            builder.Services.AddTransient<AppShell>();

            // Register To-do page and ViewModel.
            builder.Services.AddTransient<TodoListViewModel>();
            builder.Services.AddTransient<TodoListPage>();

            // Register HTTP client.
            builder.Services.AddSingleton<HttpClient>();

            // Register weather service.
            builder.Services.AddTransient<
                IWeatherService, OpenMeteoWeatherService>();

            // Register Home page and ViewModel.
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
