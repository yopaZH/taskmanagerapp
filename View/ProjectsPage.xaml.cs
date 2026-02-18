using TaskManagerApp.ViewModels;

namespace TaskManagerApp.View;

public partial class ProjectsPage : ContentPage
{
    public ProjectsPage(ProjectsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProjectsViewModel viewModel)
        {
            viewModel.LoadProjects();
        }
    }
}
