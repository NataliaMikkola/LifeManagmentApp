
using LifeManagementApp.Models;

namespace LifeManagementApp.Services;

public interface ITodoRepository
{
    Task<List<TodoItem>> GetTodosAsync();

    Task<TodoItem> AddTodoAsync(TodoItem todo);

    Task UpdateTodoAsync(TodoItem todo);

    Task DeleteTodoAsync(int id);
}
