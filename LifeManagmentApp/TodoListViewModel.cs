
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Models;

namespace LifeManagementApp.ViewModels;

public partial class TodoListViewModel : ObservableObject
{
    public ObservableCollection<TodoItem> Todos { get; } = new();

    [ObservableProperty]
    private string newTaskTitle = string.Empty;

    [ObservableProperty]
    private int newTaskPriority = 1;

    [ObservableProperty]
    private DateTime newTaskDueDate = DateTime.Today;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public TodoListViewModel()
    {
        Todos.Add(new TodoItem
        {
            Id = 1,
            Title = "Finish LifeManagementApp",
            Priority = 5,
            DueDate = DateTime.Today.AddDays(2)
        });

        Todos.Add(new TodoItem
        {
            Id = 2,
            Title = "Study C#",
            Priority = 3,
            DueDate = DateTime.Today.AddDays(1)
        });

        SortByPriority();
    }

    [RelayCommand]
    private void AddTask()
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

        var task = new TodoItem
        {
            Id = Todos.Count == 0
                ? 1
                : Todos.Max(todo => todo.Id) + 1,
            Title = NewTaskTitle.Trim(),
            Priority = NewTaskPriority,
            DueDate = NewTaskDueDate,
            IsDone = false
        };

        Todos.Add(task);

        NewTaskTitle = string.Empty;
        NewTaskPriority = 1;
        NewTaskDueDate = DateTime.Today;
        ErrorMessage = string.Empty;

        SortByPriority();
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
        {
            Todos.Add(todo);
        }
    }
}
