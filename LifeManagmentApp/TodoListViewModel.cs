
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Data;
using LifeManagementApp.Models;
using LifeManagementApp.Services;
using Microsoft.EntityFrameworkCore;

namespace LifeManagementApp.ViewModels;

public partial class TodoListViewModel : ObservableObject
{
    private readonly TodoService _todoService;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ObservableCollection<TodoItem> Todos { get; } = new();

    [ObservableProperty]
    private string newTaskTitle = string.Empty;

    [ObservableProperty]
    private int newTaskPriority = 1;

    [ObservableProperty]
    private DateTime newTaskDueDate = DateTime.Today;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public TodoListViewModel(
        TodoService todoService,
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _todoService = todoService;
        _contextFactory = contextFactory;
    }

    public async Task LoadTasksAsync()
    {
        try
        {
            await using var db =
                await _contextFactory.CreateDbContextAsync();

            await db.Database.EnsureCreatedAsync();

            var tasks = await _todoService.GetTodosAsync();

            Todos.Clear();

            foreach (var task in tasks)
                Todos.Add(task);

            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not load tasks: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task AddTaskAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTaskTitle))
        {
            ErrorMessage = "Task title cannot be empty.";
            return;
        }

        if (NewTaskPriority < 1 || NewTaskPriority > 5)
        {
            ErrorMessage = "Priority must be between 1 and 5.";
            return;
        }

        try
        {
            var task = new TodoItem
            {
                Title = NewTaskTitle.Trim(),
                Priority = NewTaskPriority,
                DueDate = NewTaskDueDate,
                IsDone = false
            };

            await _todoService.AddTodoAsync(task);

            NewTaskTitle = string.Empty;
            NewTaskPriority = 1;
            NewTaskDueDate = DateTime.Today;
            ErrorMessage = string.Empty;

            await LoadTasksAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not save task: {ex.Message}";
        }
    }

    [RelayCommand]
    private void SortByPriority()
    {
        var sorted = Todos
            .OrderByDescending(todo => todo.Priority)
            .ThenBy(todo => todo.DueDate)
            .ToList();

        UpdateTaskOrder(sorted);
    }

    [RelayCommand]
    private void SortByDueDate()
    {
        var sorted = Todos
            .OrderBy(todo => todo.DueDate)
            .ThenByDescending(todo => todo.Priority)
            .ToList();

        UpdateTaskOrder(sorted);
    }

    private void UpdateTaskOrder(List<TodoItem> sorted)
    {
        Todos.Clear();

        foreach (var todo in sorted)
            Todos.Add(todo);
    }
}
