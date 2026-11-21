
using LifeManagementApp.Views;

namespace LifeManagementApp;

public partial class AppShell : Shell
{
    public AppShell(TodoListPage todoListPage)
    {
        InitializeComponent();

        Items.Clear();

        Items.Add(new ShellContent
        {
            Title = "To-do List",
            Content = todoListPage,
            Route = "TodoListPage"
        });
    }
}
