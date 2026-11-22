
using LifeManagementApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LifeManagementApp.Services;

public class DatabaseInitializationService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public DatabaseInitializationService(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task InitializeAsync()
    {
        await using var db =
            await _contextFactory.CreateDbContextAsync();

        await db.Database.EnsureCreatedAsync();
    }
}

