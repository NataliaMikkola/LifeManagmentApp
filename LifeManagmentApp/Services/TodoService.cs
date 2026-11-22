
using LifeManagementApp.Data;
using LifeManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeManagementApp.Services;

public class TodoService : ITodoRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public TodoService(
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<TodoItem>> GetTodosAsync()
    {
        await using var db =
            await _contextFactory.CreateDbContextAsync();

        return await db.Todos
            .AsNoTracking()
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<TodoItem> AddTodoAsync(TodoItem todo)
    {
        await using var db =
            await _contextFactory.CreateDbContextAsync();

        db.Todos.Add(todo);

        await db.SaveChangesAsync();

        return todo;
    }

    public async Task UpdateTodoAsync(TodoItem todo)
    {
        await using var db =
            await _contextFactory.CreateDbContextAsync();

        db.Todos.Update(todo);

        await db.SaveChangesAsync();
    }

    public async Task DeleteTodoAsync(int id)
    {
        await using var db =
            await _contextFactory.CreateDbContextAsync();

        var todo = await db.Todos.FindAsync(id);

        if (todo == null)
            return;

        db.Todos.Remove(todo);

        await db.SaveChangesAsync();
    }
}
