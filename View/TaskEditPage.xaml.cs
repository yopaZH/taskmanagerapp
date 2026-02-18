using TaskManagerApp.ViewModels;

namespace TaskManagerApp.View;

public partial class TaskEditPage : ContentPage
{
    private TaskEditViewModel _viewModel;

    public TaskEditPage(TaskEditViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }
}
