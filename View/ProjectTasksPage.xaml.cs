using TaskManagerApp.ViewModels;

namespace TaskManagerApp.View;

public partial class ProjectTasksPage : ContentPage
{
    public ProjectTasksPage(ProjectTasksViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
