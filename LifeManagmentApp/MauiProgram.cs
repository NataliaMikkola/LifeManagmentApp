
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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Store the SQLite database in Android's app data folder.
            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "lifemanagement.db");

            // Register the database.
            builder.Services.AddDbContextFactory<AppDbContext>(
                options => options.UseSqlite(
                    $"Data Source={dbPath}"));

            // Register the task service.
            builder.Services.AddTransient<TodoService>();
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<LifeManagementApp.ViewModels.TodoListViewModel>();
            builder.Services.AddTransient<LifeManagementApp.Views.TodoListPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
