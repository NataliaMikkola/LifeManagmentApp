
using LifeManagementApp.ViewModels;

namespace LifeManagementApp.Views;

public partial class TodoListPage : ContentPage
{
    private readonly TodoListViewModel _viewModel;

    public TodoListPage(TodoListViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadTasksAsync();
    }
}
