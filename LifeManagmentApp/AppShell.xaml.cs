
using LifeManagementApp.Views;

namespace LifeManagementApp;

public partial class AppShell : Shell
{
    public AppShell(
        HomePage homePage,
        TodoListPage todoListPage)
    {
        InitializeComponent();

        var tabBar = new TabBar();

        tabBar.Items.Add(new ShellContent
        {
            Title = "Home",
            Route = "HomePage",
            Content = homePage
        });

        tabBar.Items.Add(new ShellContent
        {
            Title = "To-do List",
            Route = "TodoListPage",
            Content = todoListPage
        });

        Items.Add(tabBar);
    }
}
