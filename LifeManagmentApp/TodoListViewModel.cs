
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LifeManagementApp.Models;

namespace LifeManagementApp.ViewModels;

public partial class TodoListViewModel : ObservableObject
{
    public ObservableCollection<TodoItem> Todos { get; } = new();

    public TodoListViewModel()
    {
        Todos.Add(new TodoItem
        {
            Id = 1,
            Title = "Finish LifeManagementApp",
            Priority = 5,
            DueDate = DateTime.Today.AddDays(2),
            IsDone = false
        });

        Todos.Add(new TodoItem
        {
            Id = 2,
            Title = "Study C#",
            Priority = 3,
            DueDate = DateTime.Today.AddDays(1),
            IsDone = false
        });
    }
}
