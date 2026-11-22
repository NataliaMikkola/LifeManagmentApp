
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LifeManagementApp.Data;
using LifeManagementApp.Models;
using LifeManagementApp.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace LifeManagementApp.ViewModels;

public partial class TodoListViewModel : ObservableObject
{
    private readonly ITodoRepository _todoService;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    private List<TodoItem> _allTasks = new();
    private string _sortMode = "Priority";

    public ObservableCollection<TodoItem> Todos { get; } = new();

    public List<string> Filters { get; } =
        new() { "All", "Active", "Completed" };

    [ObservableProperty]
    private string selectedFilter = "All";

    [ObservableProperty]
    private string newTaskTitle = string.Empty;

    [ObservableProperty]
    private int newTaskPriority = 1;

    [ObservableProperty]
    private DateTime newTaskDueDate = DateTime.Today;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isEditing;

    [ObservableProperty]
    private bool isBusy;

    private int _editingTaskId;

    public string FormTitle =>
        IsEditing ? "Edit Task" : "Add New Task";

    public string SaveButtonText =>
        IsEditing ? "Save Changes" : "Add Task";

    public TodoListViewModel(
        ITodoRepository todoService,
        IDbContextFactory<AppDbContext> contextFactory)
    {
        _todoService = todoService;
        _contextFactory = contextFactory;
    }

    partial void OnIsEditingChanged(bool value)
    {
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(SaveButtonText));
    }

    partial void OnSelectedFilterChanged(string value)
    {
        RefreshList();
    }

    public async Task LoadTasksAsync()
    {
        try
        {
            await using var db =
                await _contextFactory.CreateDbContextAsync();

            await db.Database.EnsureCreatedAsync();

            _allTasks = await _todoService.GetTodosAsync();

            RefreshList();
            ErrorMessage = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Loading error: {ex.Message}";
        }
    }

    private void RefreshList()
    {
        IEnumerable<TodoItem> tasks = _allTasks;

        if (SelectedFilter == "Active")
            tasks = tasks.Where(t => !t.IsDone);
        else if (SelectedFilter == "Completed")
            tasks = tasks.Where(t => t.IsDone);

        tasks = _sortMode == "DueDate"
            ? tasks.OrderBy(t => t.DueDate)
                   .ThenByDescending(t => t.Priority)
            : tasks.OrderByDescending(t => t.Priority)
                   .ThenBy(t => t.DueDate);

        Todos.Clear();

        foreach (var task in tasks)
            Todos.Add(task);
    }

    [RelayCommand]
    private async Task SaveTaskAsync()
    {
        if (IsBusy)
            return;

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

        IsBusy = true;

        try
        {
            if (IsEditing)
            {
                var existing = _allTasks.FirstOrDefault(
                    t => t.Id == _editingTaskId);

                if (existing != null)
                {
                    existing.Title = NewTaskTitle.Trim();
                    existing.Priority = NewTaskPriority;
                    existing.DueDate = NewTaskDueDate;

                    await _todoService.UpdateTodoAsync(existing);
                }
            }
            else
            {
                var task = new TodoItem
                {
                    Title = NewTaskTitle.Trim(),
                    Priority = NewTaskPriority,
                    DueDate = NewTaskDueDate,
                    IsDone = false
                };

                await _todoService.AddTodoAsync(task);
            }

            ResetForm();
            await LoadTasksAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Saving error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void EditTask(TodoItem? task)
    {
        if (task == null)
            return;

        _editingTaskId = task.Id;

        NewTaskTitle = task.Title;
        NewTaskPriority = task.Priority;
        NewTaskDueDate = task.DueDate;

        IsEditing = true;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        ResetForm();
    }

    private void ResetForm()
    {
        NewTaskTitle = string.Empty;
        NewTaskPriority = 1;
        NewTaskDueDate = DateTime.Today;

        _editingTaskId = 0;
        IsEditing = false;
        ErrorMessage = string.Empty;
    }

    [RelayCommand]
    private async Task DeleteTaskAsync(TodoItem? task)
    {
        if (task == null || IsBusy)
            return;

        IsBusy = true;

        try
        {
            await _todoService.DeleteTodoAsync(task.Id);

            if (_editingTaskId == task.Id)
                ResetForm();

            await LoadTasksAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Delete error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ToggleCompletedAsync(TodoItem? task)
    {
        if (task == null || IsBusy)
            return;

        IsBusy = true;

        try
        {
            task.IsDone = !task.IsDone;

            await _todoService.UpdateTodoAsync(task);

            await LoadTasksAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Update error: {ex.Message}";
            await LoadTasksAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void SortByPriority()
    {
        _sortMode = "Priority";
        RefreshList();
    }

    [RelayCommand]
    private void SortByDueDate()
    {
        _sortMode = "DueDate";
        RefreshList();
    }
}
